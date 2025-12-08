namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PostInterest.v1;

/// <summary>
/// Response after posting interest.
/// </summary>
public sealed record PostFixedDepositInterestResponse(
    DefaultIdType DepositId,
    decimal InterestAmount,
    decimal TotalInterestEarned,
    string Message);
