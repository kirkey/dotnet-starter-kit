using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.WriteOff.v1;

public sealed class WriteOffLoanHandler(
    [FromKeyedServices("microfinance:loans")] IRepository<Loan> repository,
    ILogger<WriteOffLoanHandler> logger)
    : IRequestHandler<WriteOffLoanCommand, WriteOffLoanResponse>
{
    public async Task<WriteOffLoanResponse> Handle(WriteOffLoanCommand request, CancellationToken cancellationToken)
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
            throw new InvalidOperationException($"Only disbursed loans can be written off. Current status: {loan.Status}");
        }

        var writtenOffPrincipal = loan.OutstandingPrincipal;
        var writtenOffInterest = loan.OutstandingInterest;

        loan.WriteOff(request.WriteOffReason);

        await repository.UpdateAsync(loan, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogWarning(
            "Loan {LoanNumber} written off. Principal: {Principal}, Interest: {Interest}. Reason: {Reason}",
            loan.LoanNumber,
            writtenOffPrincipal,
            writtenOffInterest,
            request.WriteOffReason);

        return new WriteOffLoanResponse(loan.Id, loan.Status, writtenOffPrincipal, writtenOffInterest);
    }
}
