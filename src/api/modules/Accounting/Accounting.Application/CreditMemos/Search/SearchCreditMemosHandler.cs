using Accounting.Application.CreditMemos.Responses;

namespace Accounting.Application.CreditMemos.Search;

/// <summary>
/// Handler for searching credit memos.
/// </summary>
public sealed class SearchCreditMemosHandler(
    IReadRepository<CreditMemo> repository)
    : IRequestHandler<SearchCreditMemosQuery, PagedList<CreditMemoResponse>>
{
    public async Task<PagedList<CreditMemoResponse>> Handle(SearchCreditMemosQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCreditMemosSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        return new PagedList<CreditMemoResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
