// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/InsuranceProducts/Search/v1/SearchInsuranceProductsCommand.cs
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Search.v1;

/// <summary>
/// Command for searching insurance products with pagination and filters.
/// </summary>
public class SearchInsuranceProductsCommand : PaginationFilter, IRequest<PagedList<InsuranceProductResponse>>
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
    /// Filter by insurance type.
    /// </summary>
    public string? InsuranceType { get; set; }

    /// <summary>
    /// Filter by provider.
    /// </summary>
    public string? Provider { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by whether mandatory with loan.
    /// </summary>
    public bool? MandatoryWithLoan { get; set; }
}

