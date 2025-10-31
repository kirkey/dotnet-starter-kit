namespace Accounting.Application.RetainedEarnings.Responses;

public record RetainedEarningsResponse
{
    public DefaultIdType Id { get; init; }
    public int FiscalYear { get; init; }
    public decimal BeginningBalance { get; init; }
    public decimal NetIncome { get; init; }
    public decimal Dividends { get; init; }
    public decimal EndingBalance { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsClosed { get; init; }
    public string? Description { get; init; }
}

