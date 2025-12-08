namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.PostInterest.v1;

/// <summary>
/// Response after posting interest.
/// </summary>
public sealed record PostInterestResponse(
    DefaultIdType AccountId,
    decimal InterestAmount,
    decimal NewBalance,
    decimal TotalInterestEarned,
    string Message);
