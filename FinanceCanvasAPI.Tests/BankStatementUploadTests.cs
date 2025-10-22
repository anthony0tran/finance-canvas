namespace FinanceCanvasAPI.Tests;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class BankStatementUploadTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task UploadPdf_WithAntiforgeryToken_ReturnsOk()
    {
        var client = factory.CreateClient();

        // 1. Get antiforgery token
        var tokenResponse = await client.GetAsync("/antiforgery/token");
        var token = await tokenResponse.Content.ReadAsStringAsync();

        // 2. Load PDF file from TestData
        var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
        var files = Directory.Exists(testDataPath) ? Directory.GetFiles(testDataPath) : [];

        byte[] pdfBytes;
        string fileName;

        if (files.Length > 0)
        {
            fileName = Path.GetFileName(files[0]);
            pdfBytes = File.ReadAllBytes(files[0]);
        }
        else
        {
            fileName = "empty.pdf";
            pdfBytes = [];
        }

        var pdfContent = new ByteArrayContent(pdfBytes);
        pdfContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        var form = new MultipartFormDataContent();
        form.Add(pdfContent, "file", fileName);

        // 3. Add antiforgery token header
        form.Headers.Add("RequestVerificationToken", token);

        // 4. Upload PDF
        var response = await client.PostAsync("/bankstatements/upload", form);

        // 5. Assert
        response.EnsureSuccessStatusCode();
    }
}
