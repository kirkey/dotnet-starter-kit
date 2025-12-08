using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Reverse.v1;

/// <summary>
/// Command to reverse a loan repayment.
/// </summary>
public sealed record ReverseLoanRepaymentCommand(DefaultIdType LoanRepaymentId, string Reason) : IRequest<ReverseLoanRepaymentResponse>;
