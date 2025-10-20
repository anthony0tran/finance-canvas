using FinanceCanvasAPI.Context;
using FinanceCanvasAPI.Context.Entities;
using FinanceCanvasAPI.Models;
using FinanceCanvasAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var encryptionKey = builder.Configuration["db:SQLCIPHER_KEY"]
                    ?? throw new InvalidOperationException("Missing DB connection string");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<LiteDB.LiteDatabase>(_ =>
    LiteDbFactory.Create(encryptionKey));
builder.Services.AddScoped<BankStatementRepository>();


builder.Services.AddScoped<BankStatementRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// endpoints
app.MapGet("/bankstatements", ([FromServices] BankStatementRepository repo) =>
{
    var statements = repo
        .GetBankStatements()
        .FindAll()
        .ToList();

    return Results.Ok(statements);
});

app.MapPost("/bankstatements", (BankStatementDto statementDto, [FromServices] BankStatementRepository repo) =>
{
    var statement = new BankStatement
    {
        Date = statementDto.Date,
        SerialNumber = statementDto.SerialNumber
    };

    repo.Insert(statement);
    return Task.FromResult(Results.Created($"/bankstatements/{statement.Id}", statement));
});

app.Run();