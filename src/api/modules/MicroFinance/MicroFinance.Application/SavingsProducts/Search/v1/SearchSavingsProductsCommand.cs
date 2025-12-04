using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Search.v1;

/// <summary>
/// Command to search savings products with filters and pagination.
/// </summary>
public class SearchSavingsProductsCommand : PaginationFilter, IRequest<PagedList<SavingsProductResponse>>
{
    /// <summary>
    /// Filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Filter by products that allow overdraft.
    /// </summary>
    public bool? AllowOverdraft { get; set; }

    /// <summary>
    /// Filter by minimum interest rate.
    /// </summary>
    public decimal? MinInterestRate { get; set; }

    /// <summary>
    /// Filter by maximum interest rate.
    /// </summary>
    public decimal? MaxInterestRate { get; set; }
}
