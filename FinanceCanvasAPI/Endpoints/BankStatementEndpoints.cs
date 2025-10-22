using FinanceCanvasAPI.Context.Entities;
using FinanceCanvasAPI.Models;
using FinanceCanvasAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using UglyToad.PdfPig;

namespace FinanceCanvasAPI.Endpoints;

public static class BankStatementEndpoints
{
    public static void MapBankStatementEndpoints(this WebApplication app)
    {
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

        app.MapPost("/bankstatements/upload", async (IFormFile file) =>
        {
            if (file.Length == 0 ||
                !(file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) ||
                  file.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)))
                return Results.BadRequest("Please upload a valid PDF or ZIP file.");

            if (file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                await using var pdfStream = file.OpenReadStream();
                using var pdf = PdfDocument.Open(pdfStream);
                var text = string.Join("\n", pdf.GetPages().Select(p => p.Text));
                return Results.Ok(new { type = "pdf", content = text });
            }

            await using var zipStream = file.OpenReadStream();
            using var archive =
                new System.IO.Compression.ZipArchive(zipStream, System.IO.Compression.ZipArchiveMode.Read);
            var fileNames = archive.Entries.Select(e => e.FullName).ToList();
            return Results.Ok(new { type = "zip", files = fileNames });
        });
    }
}