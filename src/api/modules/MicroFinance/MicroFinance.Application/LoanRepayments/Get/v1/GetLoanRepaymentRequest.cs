using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Get.v1;

/// <summary>
/// Request to retrieve a loan repayment by its ID.
/// </summary>
/// <param name="Id">The unique identifier of the loan repayment.</param>
public record GetLoanRepaymentRequest(DefaultIdType Id) : IRequest<LoanRepaymentResponse>;
