using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.UpdateNav.v1;

public sealed class UpdateInvestmentProductNavHandler(
    [FromKeyedServices("microfinance:investmentproducts")] IRepository<InvestmentProduct> repository,
    ILogger<UpdateInvestmentProductNavHandler> logger)
    : IRequestHandler<UpdateInvestmentProductNavCommand, UpdateInvestmentProductNavResponse>
{
    public async Task<UpdateInvestmentProductNavResponse> Handle(UpdateInvestmentProductNavCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.FirstOrDefaultAsync(new InvestmentProductByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Investment product {request.Id} not found");

        product.UpdateNav(request.NewNav, request.NavDate);
        await repository.UpdateAsync(product, cancellationToken);

        logger.LogInformation("Updated NAV for investment product {Id} to {Nav}", product.Id, request.NewNav);

        return new UpdateInvestmentProductNavResponse(product.Id, product.CurrentNav, product.NavDate ?? request.NavDate);
    }
}
