namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Assign.v1;

/// <summary>
/// Response after assigning a loan application.
/// </summary>
public sealed record AssignLoanApplicationResponse(Guid Id, Guid AssignedOfficerId, string Status);
