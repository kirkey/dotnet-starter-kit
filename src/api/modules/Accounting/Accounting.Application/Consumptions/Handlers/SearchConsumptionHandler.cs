using Accounting.Application.Consumptions.Queries;
using Accounting.Application.Consumptions.Responses;

namespace Accounting.Application.Consumptions.Handlers;

/// <summary>
/// Handler for searching consumptions with optional filters and pagination.
/// </summary>
public class SearchConsumptionHandler(
    [FromKeyedServices("accounting:consumptions")] IReadRepository<Consumption> repository)
    : IRequestHandler<SearchConsumptionQuery, PagedList<ConsumptionResponse>>
{
    /// <summary>
    /// Handles the query to search consumptions applying filters and pagination.
    /// </summary>
    /// <param name="request">The search query containing filter criteria and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paged list of consumption responses.</returns>
    public async Task<PagedList<ConsumptionResponse>> Handle(SearchConsumptionQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchConsumptionSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ConsumptionResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
