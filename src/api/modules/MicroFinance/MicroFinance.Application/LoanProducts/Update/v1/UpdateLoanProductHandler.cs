using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Update.v1;

public sealed class UpdateLoanProductHandler(
    [FromKeyedServices("microfinance:loanproducts")] IRepository<LoanProduct> repository,
    ILogger<UpdateLoanProductHandler> logger)
    : IRequestHandler<UpdateLoanProductCommand, UpdateLoanProductResponse>
{
    public async Task<UpdateLoanProductResponse> Handle(UpdateLoanProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var loanProduct = await repository.FirstOrDefaultAsync(
            new LoanProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (loanProduct is null)
        {
            throw new NotFoundException($"Loan product with ID {request.Id} not found.");
        }

        loanProduct.Update(
            request.Name,
            request.Description,
            request.MinLoanAmount,
            request.MaxLoanAmount,
            request.InterestRate,
            request.InterestMethod,
            request.MinTermMonths,
            request.MaxTermMonths,
            request.RepaymentFrequency,
            request.GracePeriodDays,
            request.LatePenaltyRate
        );

        await repository.UpdateAsync(loanProduct, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Loan product {LoanProductId} updated", loanProduct.Id);

        return new UpdateLoanProductResponse(loanProduct.Id);
    }
}
