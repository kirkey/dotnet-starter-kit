using Accounting.Application.Meters.Responses;

namespace Accounting.Application.Meters.Search.v1;

/// <summary>
/// Handler for searching meters with filters and pagination.
/// </summary>
public sealed class SearchMetersHandler(
    [FromKeyedServices("accounting:meters")] IReadRepository<Meter> repository)
    : IRequestHandler<SearchMetersRequest, PagedList<MeterResponse>>
{
    public async Task<PagedList<MeterResponse>> Handle(SearchMetersRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchMetersSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<MeterResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}

