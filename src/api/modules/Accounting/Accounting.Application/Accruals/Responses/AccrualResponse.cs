namespace Accounting.Application.Accruals.Responses;

public sealed class AccrualResponse(
    DefaultIdType id,
    string accrualNumber,
    DateTime accrualDate,
    decimal amount,
    string? description,
    bool isReversed,
    DateTime? reversalDate)
{
    public DefaultIdType Id { get; init; } = id;
    public string AccrualNumber { get; init; } = accrualNumber;
    public DateTime AccrualDate { get; init; } = accrualDate;
    public decimal Amount { get; init; } = amount;
    public string? Description { get; init; } = description;
    public bool IsReversed { get; init; } = isReversed;
    public DateTime? ReversalDate { get; init; } = reversalDate;
}
