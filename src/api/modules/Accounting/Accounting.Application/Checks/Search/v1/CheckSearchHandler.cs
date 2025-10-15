using Accounting.Domain.Entities;

namespace Accounting.Application.Checks.Search.v1;

/// <summary>
/// Handler for searching checks with filtering and pagination.
/// </summary>
public sealed class CheckSearchHandler(
    [FromKeyedServices("accounting:checks")] IReadRepository<Check> repository)
    : IRequestHandler<CheckSearchQuery, PagedList<CheckSearchResponse>>
{
    public async Task<PagedList<CheckSearchResponse>> Handle(CheckSearchQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new CheckSearchSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CheckSearchResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
