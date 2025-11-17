using FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Search.v1;

/// <summary>
/// Handler for SearchTaxesRequest.
/// Searches and paginates tax master configurations.
/// </summary>
public sealed class SearchTaxesHandler(
    [FromKeyedServices("hr:taxes")] IReadRepository<TaxMaster> repository)
    : IRequestHandler<SearchTaxesRequest, PagedList<TaxDto>>
{
    /// <summary>
    /// Handles the search taxes query.
    /// </summary>
    /// <param name="request">Search query with filters and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of tax DTOs matching filters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    public async Task<PagedList<TaxDto>> Handle(SearchTaxesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchTaxesSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<TaxDto>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
