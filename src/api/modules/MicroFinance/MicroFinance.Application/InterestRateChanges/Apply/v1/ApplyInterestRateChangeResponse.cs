namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Apply.v1;

/// <summary>
/// Response after applying an interest rate change.
/// </summary>
public sealed record ApplyInterestRateChangeResponse(
    DefaultIdType Id,
    string Status,
    DefaultIdType LoanId,
    decimal NewRate,
    DateOnly AppliedDate);
