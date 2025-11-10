using Accounting.Application.Consumptions.Responses;

namespace Accounting.Application.Consumptions.Search.v1;

/// <summary>
/// Handler for searching consumption records with filters and pagination.
/// </summary>
public sealed class SearchConsumptionsHandler(
    [FromKeyedServices("accounting:consumptions")] IReadRepository<Consumption> repository)
    : IRequestHandler<SearchConsumptionsRequest, PagedList<ConsumptionResponse>>
{
    public async Task<PagedList<ConsumptionResponse>> Handle(SearchConsumptionsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchConsumptionsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ConsumptionResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}

