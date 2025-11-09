using Accounting.Application.RetainedEarnings.Queries;
using Accounting.Application.RetainedEarnings.Responses;

namespace Accounting.Application.RetainedEarnings.Search.v1;

/// <summary>
/// Handler for searching retained earnings with filters and pagination.
/// </summary>
public sealed class SearchRetainedEarningsHandler(
    ILogger<SearchRetainedEarningsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<Domain.Entities.RetainedEarnings> repository)
    : IRequestHandler<SearchRetainedEarningsRequest, PagedList<RetainedEarningsResponse>>
{
    public async Task<PagedList<RetainedEarningsResponse>> Handle(SearchRetainedEarningsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new RetainedEarningsSearchSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} retained earnings records", items.Count);

        return new PagedList<RetainedEarningsResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
