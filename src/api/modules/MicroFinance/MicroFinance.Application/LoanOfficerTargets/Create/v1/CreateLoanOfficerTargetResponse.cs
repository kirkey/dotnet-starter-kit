namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Create.v1;

/// <summary>
/// Response from creating a loan officer target.
/// </summary>
public sealed record CreateLoanOfficerTargetResponse(Guid Id, string TargetType, decimal TargetValue, string Status);
