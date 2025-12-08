namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Review.v1;

/// <summary>
/// Response after completing loan application review.
/// </summary>
public sealed record ReviewLoanApplicationResponse(DefaultIdType Id, string Status);
