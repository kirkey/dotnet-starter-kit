namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Withdraw.v1;

/// <summary>
/// Response after withdrawing a loan application.
/// </summary>
public sealed record WithdrawLoanApplicationResponse(DefaultIdType Id, string Status);
