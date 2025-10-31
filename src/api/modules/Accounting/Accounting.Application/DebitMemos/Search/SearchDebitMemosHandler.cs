using Accounting.Application.DebitMemos.Responses;

namespace Accounting.Application.DebitMemos.Search;

/// <summary>
/// Handler for searching debit memos.
/// </summary>
public sealed class SearchDebitMemosHandler(
    IReadRepository<DebitMemo> repository)
    : IRequestHandler<SearchDebitMemosQuery, PagedList<DebitMemoResponse>>
{
    public async Task<PagedList<DebitMemoResponse>> Handle(SearchDebitMemosQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchDebitMemosSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        return new PagedList<DebitMemoResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
