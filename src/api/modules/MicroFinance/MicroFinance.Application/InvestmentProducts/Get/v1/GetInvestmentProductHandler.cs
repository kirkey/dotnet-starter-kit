using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Get.v1;

public sealed class GetInvestmentProductHandler(
    [FromKeyedServices("microfinance:investmentproducts")] IReadRepository<InvestmentProduct> repository,
    ILogger<GetInvestmentProductHandler> logger)
    : IRequestHandler<GetInvestmentProductRequest, InvestmentProductResponse>
{
    public async Task<InvestmentProductResponse> Handle(GetInvestmentProductRequest request, CancellationToken cancellationToken)
    {
        var product = await repository.FirstOrDefaultAsync(new InvestmentProductByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Investment product {request.Id} not found");

        logger.LogInformation("Retrieved investment product {Id}", product.Id);

        return new InvestmentProductResponse(
            product.Id,
            product.Name,
            product.Code,
            product.ProductType,
            product.Status,
            product.RiskLevel,
            product.MinimumInvestment,
            product.MaximumInvestment,
            product.ManagementFeePercent,
            product.PerformanceFeePercent,
            product.EntryLoadPercent,
            product.ExitLoadPercent,
            product.ExpectedReturnMin,
            product.ExpectedReturnMax,
            product.LockInPeriodDays,
            product.CurrentNav,
            product.NavDate,
            product.TotalAum,
            product.TotalInvestors,
            product.FundManager,
            product.YtdReturn,
            product.OneYearReturn,
            product.ThreeYearReturn,
            product.AllowPartialRedemption,
            product.AllowSip,
            product.Description);
    }
}
