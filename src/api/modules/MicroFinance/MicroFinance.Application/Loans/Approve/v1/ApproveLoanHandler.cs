using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Approve.v1;

public sealed class ApproveLoanHandler(
    [FromKeyedServices("microfinance:loans")] IRepository<Loan> repository,
    ILogger<ApproveLoanHandler> logger)
    : IRequestHandler<ApproveLoanCommand, ApproveLoanResponse>
{
    public async Task<ApproveLoanResponse> Handle(ApproveLoanCommand request, CancellationToken cancellationToken)
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
            throw new InvalidOperationException($"Only pending loans can be approved. Current status: {loan.Status}");
        }

        loan.Approve(DateOnly.FromDateTime(DateTime.UtcNow));

        await repository.UpdateAsync(loan, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Loan {LoanNumber} approved", loan.LoanNumber);

        return new ApproveLoanResponse(loan.Id, loan.Status, loan.ApprovalDate!.Value);
    }
}
