namespace Accounting.Application.Accruals.Create;

public sealed class CreateAccrualResponse(
    DefaultIdType id,
    string accrualNumber)
{
    public DefaultIdType Id { get; init; } = id;
    public string AccrualNumber { get; init; } = accrualNumber;
}

