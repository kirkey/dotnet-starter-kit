using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Submit.v1;

/// <summary>
/// Command to submit a loan application for review.
/// </summary>
public sealed record SubmitLoanApplicationCommand(DefaultIdType Id) : IRequest<SubmitLoanApplicationResponse>;
