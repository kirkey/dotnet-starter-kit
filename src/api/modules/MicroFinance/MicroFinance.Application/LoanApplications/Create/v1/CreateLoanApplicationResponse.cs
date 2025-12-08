namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Create.v1;

/// <summary>
/// Response after creating a loan application.
/// </summary>
public sealed record CreateLoanApplicationResponse(DefaultIdType Id, string ApplicationNumber);
