namespace FinanceCanvasAPI.Context.Entities;

public record BankStatement
{
    public Guid Id { get; init; }
    public DateOnly Date { get; init; }
    public required string SerialNumber { get; init; }
}