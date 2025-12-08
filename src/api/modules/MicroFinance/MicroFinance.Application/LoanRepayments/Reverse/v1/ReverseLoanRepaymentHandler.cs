using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Reverse.v1;

/// <summary>
/// Handler for reversing a loan repayment.
/// </summary>
public sealed class ReverseLoanRepaymentHandler(
    [FromKeyedServices("microfinance:loanrepayments")] IRepository<LoanRepayment> repository,
    ILogger<ReverseLoanRepaymentHandler> logger)
    : IRequestHandler<ReverseLoanRepaymentCommand, ReverseLoanRepaymentResponse>
{
    public async Task<ReverseLoanRepaymentResponse> Handle(ReverseLoanRepaymentCommand request, CancellationToken cancellationToken)
    {
        var loanRepayment = await repository.GetByIdAsync(request.LoanRepaymentId, cancellationToken)
            ?? throw new NotFoundException($"Loan repayment with ID {request.LoanRepaymentId} not found.");

        loanRepayment.Reverse(request.Reason);

        await repository.UpdateAsync(loanRepayment, cancellationToken);
        logger.LogInformation("Reversed loan repayment {LoanRepaymentId}", request.LoanRepaymentId);

        return new ReverseLoanRepaymentResponse(loanRepayment.Id, loanRepayment.Status, "Loan repayment reversed successfully.");
    }
}
