namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Reject.v1;

/// <summary>
/// Response after rejecting a loan application.
/// </summary>
public sealed record RejectLoanApplicationResponse(Guid Id, string Status);
