using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Update.v1;

public sealed class UpdateInvestmentProductHandler(
    [FromKeyedServices("microfinance:investmentproducts")] IRepository<InvestmentProduct> repository,
    ILogger<UpdateInvestmentProductHandler> logger)
    : IRequestHandler<UpdateInvestmentProductCommand, UpdateInvestmentProductResponse>
{
    public async Task<UpdateInvestmentProductResponse> Handle(
        UpdateInvestmentProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await repository.FirstOrDefaultAsync(
            new InvestmentProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (product is null)
        {
            throw new NotFoundException($"Investment product with ID {request.Id} not found.");
        }

        product.Update(
            request.Name,
            request.Description,
            request.MinimumInvestment,
            request.MaximumInvestment,
            request.ManagementFeePercent,
            request.PerformanceFeePercent,
            request.EntryLoadPercent,
            request.ExitLoadPercent,
            request.MinimumHoldingDays,
            request.FundManager,
            request.Benchmark,
            request.AllowPartialRedemption,
            request.AllowSip,
            request.DisplayOrder);

        await repository.UpdateAsync(product, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Investment product updated: {ProductId}", request.Id);

        return new UpdateInvestmentProductResponse(product.Id);
    }
}
