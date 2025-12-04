namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Submit.v1;

/// <summary>
/// Response after submitting a loan application.
/// </summary>
public sealed record SubmitLoanApplicationResponse(Guid Id, string Status);
