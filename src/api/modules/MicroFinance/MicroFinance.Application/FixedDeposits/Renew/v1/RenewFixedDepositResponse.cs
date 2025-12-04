namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Renew.v1;

/// <summary>
/// Response after renewing fixed deposit.
/// </summary>
public sealed record RenewFixedDepositResponse(
    Guid DepositId,
    string Status,
    int TermMonths,
    decimal InterestRate,
    DateOnly NewMaturityDate,
    string Message);
