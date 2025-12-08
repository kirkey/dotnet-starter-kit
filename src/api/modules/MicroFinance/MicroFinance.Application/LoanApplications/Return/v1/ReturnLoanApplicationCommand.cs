using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Return.v1;

/// <summary>
/// Command to return a loan application to the applicant for corrections.
/// </summary>
public sealed record ReturnLoanApplicationCommand(
    DefaultIdType LoanApplicationId, 
    string Reason,
    string? Notes = null) : IRequest<ReturnLoanApplicationResponse>;
