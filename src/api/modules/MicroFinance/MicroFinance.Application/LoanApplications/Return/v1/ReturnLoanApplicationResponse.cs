namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Return.v1;

/// <summary>
/// Response after returning a loan application.
/// </summary>
public sealed record ReturnLoanApplicationResponse(DefaultIdType LoanApplicationId, string Status, string Message);
