// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/InsuranceProducts/Search/v1/SearchInsuranceProductsHandler.cs
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Search.v1;

/// <summary>
/// Handler for searching insurance products.
/// </summary>
public sealed class SearchInsuranceProductsHandler(
    [FromKeyedServices("microfinance:insuranceproducts")] IReadRepository<InsuranceProduct> repository)
    : IRequestHandler<SearchInsuranceProductsCommand, PagedList<InsuranceProductResponse>>
{
    public async Task<PagedList<InsuranceProductResponse>> Handle(SearchInsuranceProductsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInsuranceProductsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InsuranceProductResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

