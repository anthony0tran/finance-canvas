namespace FinanceCanvasAPI.Models;

public record BankStatementDto
{
    public DateOnly Date { get; init; }
    public required string SerialNumber { get; init; }
}