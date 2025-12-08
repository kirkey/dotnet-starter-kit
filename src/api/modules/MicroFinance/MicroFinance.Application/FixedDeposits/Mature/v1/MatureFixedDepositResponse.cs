namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Mature.v1;

/// <summary>
/// Response after maturing fixed deposit.
/// </summary>
public sealed record MatureFixedDepositResponse(
    DefaultIdType DepositId,
    string Status,
    DateOnly? ClosedDate,
    string Message);
