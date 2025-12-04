using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Update.v1;

public sealed class UpdateLoanHandler(
    [FromKeyedServices("microfinance:loans")] IRepository<Loan> loanRepository,
    ILogger<UpdateLoanHandler> logger)
    : IRequestHandler<UpdateLoanCommand, UpdateLoanResponse>
{
    public async Task<UpdateLoanResponse> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var loan = await loanRepository.FirstOrDefaultAsync(
            new LoanByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (loan is null)
        {
            throw new NotFoundException($"Loan with ID {request.Id} not found.");
        }

        // Update the loan using domain method (will throw if not pending)
        loan.Update(
            request.InterestRate,
            request.TermMonths,
            request.RepaymentFrequency,
            request.Purpose);

        await loanRepository.UpdateAsync(loan, cancellationToken).ConfigureAwait(false);
        await loanRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Loan {LoanId} updated successfully", request.Id);

        return new UpdateLoanResponse(loan.Id);
    }
}
