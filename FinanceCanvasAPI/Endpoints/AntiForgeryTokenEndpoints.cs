using Microsoft.AspNetCore.Antiforgery;

namespace FinanceCanvasAPI.Endpoints;

public static class AntiForgeryTokenEndpoints
{
    public static void MapAntiForgeryTokenEndpoints(this WebApplication app)
    {
        app.MapGet("/antiforgery/token", (IAntiforgery antiforgery, HttpContext context) =>
        {
            var tokens = antiforgery.GetAndStoreTokens(context);
            return Results.Ok(new { RequestVerificationToken = tokens.RequestToken });
        });
    }
}