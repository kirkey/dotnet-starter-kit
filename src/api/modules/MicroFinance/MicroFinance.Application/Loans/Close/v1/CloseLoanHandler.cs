using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Close.v1;

public sealed class CloseLoanHandler(
    [FromKeyedServices("microfinance:loans")] IRepository<Loan> repository,
    ILogger<CloseLoanHandler> logger)
    : IRequestHandler<CloseLoanCommand, CloseLoanResponse>
{
    public async Task<CloseLoanResponse> Handle(CloseLoanCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var loan = await repository.FirstOrDefaultAsync(
            new LoanByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (loan is null)
        {
            throw new NotFoundException($"Loan with ID {request.Id} not found.");
        }

        if (loan.Status != "DISBURSED")
        {
            throw new InvalidOperationException($"Only disbursed loans can be closed. Current status: {loan.Status}");
        }

        // Check if there's any outstanding balance
        if (loan.OutstandingPrincipal > 0 || loan.OutstandingInterest > 0)
        {
            throw new InvalidOperationException(
                $"Cannot close loan with outstanding balance. Outstanding Principal: {loan.OutstandingPrincipal}, Outstanding Interest: {loan.OutstandingInterest}");
        }

        var closeDate = DateOnly.FromDateTime(DateTime.UtcNow);
        loan.Close(closeDate);

        await repository.UpdateAsync(loan, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Loan {LoanNumber} closed", loan.LoanNumber);

        return new CloseLoanResponse(loan.Id, loan.Status, closeDate);
    }
}
