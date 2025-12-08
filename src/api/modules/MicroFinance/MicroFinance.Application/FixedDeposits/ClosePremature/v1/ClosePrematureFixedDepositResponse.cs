namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.ClosePremature.v1;

/// <summary>
/// Response after premature closure.
/// </summary>
public sealed record ClosePrematureFixedDepositResponse(
    DefaultIdType DepositId,
    string Status,
    DateOnly? ClosedDate,
    string Message);
