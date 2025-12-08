// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/InvestmentProducts/Search/v1/SearchInvestmentProductsSpecs.cs
using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Search.v1;

/// <summary>
/// Specification for searching investment products.
/// </summary>
public sealed class SearchInvestmentProductsSpecs : Specification<InvestmentProduct, InvestmentProductResponse>
{
    public SearchInvestmentProductsSpecs(SearchInvestmentProductsCommand command)
    {
        Query.OrderBy(p => p.Name);

        if (!string.IsNullOrEmpty(command.Name))
        {
            Query.Where(p => p.Name.Contains(command.Name));
        }

        if (!string.IsNullOrEmpty(command.Code))
        {
            Query.Where(p => p.Code.Contains(command.Code));
        }

        if (!string.IsNullOrEmpty(command.ProductType))
        {
            Query.Where(p => p.ProductType == command.ProductType);
        }

        if (!string.IsNullOrEmpty(command.Status))
        {
            Query.Where(p => p.Status == command.Status);
        }

        if (!string.IsNullOrEmpty(command.RiskLevel))
        {
            Query.Where(p => p.RiskLevel == command.RiskLevel);
        }

        if (command.AllowSip.HasValue)
        {
            Query.Where(p => p.AllowSip == command.AllowSip.Value);
        }

        Query.Skip(command.PageSize * (command.PageNumber - 1))
            .Take(command.PageSize);

        Query.Select(p => new InvestmentProductResponse(
            p.Id,
            p.Name,
            p.Code,
            p.ProductType,
            p.Status,
            p.RiskLevel,
            p.MinimumInvestment,
            p.MaximumInvestment,
            p.ManagementFeePercent,
            p.PerformanceFeePercent,
            p.EntryLoadPercent,
            p.ExitLoadPercent,
            p.ExpectedReturnMin,
            p.ExpectedReturnMax,
            p.LockInPeriodDays,
            p.CurrentNav,
            p.NavDate,
            p.TotalAum,
            p.TotalInvestors,
            p.FundManager,
            p.YtdReturn,
            p.OneYearReturn,
            p.ThreeYearReturn,
            p.AllowPartialRedemption,
            p.AllowSip,
            p.Description));
    }
}

