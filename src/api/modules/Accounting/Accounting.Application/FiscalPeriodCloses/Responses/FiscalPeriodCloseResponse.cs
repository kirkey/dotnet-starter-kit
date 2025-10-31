namespace Accounting.Application.FiscalPeriodCloses.Responses;

public record FiscalPeriodCloseResponse
{
    public DefaultIdType Id { get; init; }
    public string CloseNumber { get; init; } = string.Empty;
    public DateTime PeriodStartDate { get; init; }
    public DateTime PeriodEndDate { get; init; }
    public DateTime? CloseDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string CloseType { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

