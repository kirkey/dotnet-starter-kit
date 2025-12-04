using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Withdraw.v1;

/// <summary>
/// Command to withdraw a loan application by the applicant.
/// </summary>
public sealed record WithdrawLoanApplicationCommand(Guid Id) : IRequest<WithdrawLoanApplicationResponse>;
