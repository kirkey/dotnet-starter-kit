using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Reject.v1;

public sealed class RejectLoanHandler(
    [FromKeyedServices("microfinance:loans")] IRepository<Loan> repository,
    ILogger<RejectLoanHandler> logger)
    : IRequestHandler<RejectLoanCommand, RejectLoanResponse>
{
    public async Task<RejectLoanResponse> Handle(RejectLoanCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var loan = await repository.FirstOrDefaultAsync(
            new LoanByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (loan is null)
        {
            throw new NotFoundException($"Loan with ID {request.Id} not found.");
        }

        if (loan.Status != "PENDING")
        {
            throw new InvalidOperationException($"Only pending loans can be rejected. Current status: {loan.Status}");
        }

        loan.Reject(request.RejectionReason);

        await repository.UpdateAsync(loan, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Loan {LoanNumber} rejected. Reason: {RejectionReason}", loan.LoanNumber, request.RejectionReason);

        return new RejectLoanResponse(loan.Id, loan.Status);
    }
}
