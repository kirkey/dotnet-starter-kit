using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Activate.v1;

public sealed class ActivateInvestmentProductHandler(
    [FromKeyedServices("microfinance:investmentproducts")] IRepository<InvestmentProduct> repository,
    ILogger<ActivateInvestmentProductHandler> logger)
    : IRequestHandler<ActivateInvestmentProductCommand, ActivateInvestmentProductResponse>
{
    public async Task<ActivateInvestmentProductResponse> Handle(
        ActivateInvestmentProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await repository.FirstOrDefaultAsync(
            new InvestmentProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (product is null)
        {
            throw new NotFoundException($"Investment product with ID {request.Id} not found.");
        }

        product.Activate();

        await repository.UpdateAsync(product, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Investment product activated: {ProductId}", request.Id);

        return new ActivateInvestmentProductResponse(product.Id);
    }
}
