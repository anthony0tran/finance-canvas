using FinanceCanvasAPI.Context;
using FinanceCanvasAPI.Endpoints;
using FinanceCanvasAPI.Repository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var encryptionKey = builder.Configuration["db:SQLCIPHER_KEY"]
                    ?? throw new InvalidOperationException("Missing DB connection string");

builder.Services.AddOpenApi();
builder.Services.AddAntiforgery();
builder.Services.AddScoped<LiteDB.LiteDatabase>(_ =>
    LiteDbFactory.Create(encryptionKey));
builder.Services.AddScoped<BankStatementRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapAntiForgeryTokenEndpoints();
app.MapBankStatementEndpoints();

app.Run();