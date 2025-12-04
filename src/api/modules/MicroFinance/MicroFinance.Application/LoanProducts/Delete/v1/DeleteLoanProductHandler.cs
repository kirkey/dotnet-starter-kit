using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Delete.v1;

public sealed class DeleteLoanProductHandler(
    [FromKeyedServices("microfinance:loanproducts")] IRepository<LoanProduct> repository,
    [FromKeyedServices("microfinance:loans")] IRepository<Loan> loanRepository,
    ILogger<DeleteLoanProductHandler> logger)
    : IRequestHandler<DeleteLoanProductCommand, Unit>
{
    public async Task<Unit> Handle(DeleteLoanProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var loanProduct = await repository.FirstOrDefaultAsync(
            new LoanProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (loanProduct is null)
        {
            throw new NotFoundException($"Loan product with ID {request.Id} not found.");
        }

        // Check if there are any loans using this product
        var hasLoans = await loanRepository.AnyAsync(
            new LoansByProductIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (hasLoans)
        {
            throw new InvalidOperationException("Cannot delete loan product that has associated loans. Please deactivate the product instead.");
        }

        await repository.DeleteAsync(loanProduct, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Loan product {LoanProductId} deleted", request.Id);

        return Unit.Value;
    }
}
