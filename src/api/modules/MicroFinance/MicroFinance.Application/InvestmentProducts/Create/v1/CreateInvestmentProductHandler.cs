using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Create.v1;

public sealed class CreateInvestmentProductHandler(
    [FromKeyedServices("microfinance:investmentproducts")] IRepository<InvestmentProduct> repository,
    ILogger<CreateInvestmentProductHandler> logger)
    : IRequestHandler<CreateInvestmentProductCommand, CreateInvestmentProductResponse>
{
    public async Task<CreateInvestmentProductResponse> Handle(CreateInvestmentProductCommand request, CancellationToken cancellationToken)
    {
        var product = InvestmentProduct.Create(
            request.Name,
            request.Code,
            request.ProductType,
            request.RiskLevel,
            request.MinimumInvestment,
            request.ManagementFeePercent,
            request.ExpectedReturnMin,
            request.ExpectedReturnMax,
            request.LockInPeriodDays,
            request.Description);

        await repository.AddAsync(product, cancellationToken);
        logger.LogInformation("Investment product {Code} created with ID {Id}", product.Code, product.Id);

        return new CreateInvestmentProductResponse(product.Id, product.Code, product.ProductType, product.RiskLevel);
    }
}
