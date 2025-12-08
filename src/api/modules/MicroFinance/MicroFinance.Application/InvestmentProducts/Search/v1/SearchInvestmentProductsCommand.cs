// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/InvestmentProducts/Search/v1/SearchInvestmentProductsCommand.cs
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Search.v1;

/// <summary>
/// Command for searching investment products with pagination and filters.
/// </summary>
public class SearchInvestmentProductsCommand : PaginationFilter, IRequest<PagedList<InvestmentProductResponse>>
{
    /// <summary>
    /// Filter by product name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Filter by product code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Filter by product type.
    /// </summary>
    public string? ProductType { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by risk level.
    /// </summary>
    public string? RiskLevel { get; set; }

    /// <summary>
    /// Filter by whether SIP is allowed.
    /// </summary>
    public bool? AllowSip { get; set; }
}

