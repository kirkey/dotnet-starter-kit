namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Approve.v1;

/// <summary>
/// Response after approving a loan application.
/// </summary>
public sealed record ApproveLoanApplicationResponse(
    Guid Id, 
    decimal ApprovedAmount, 
    int ApprovedTermMonths,
    decimal ApprovedInterestRate,
    string Status);
