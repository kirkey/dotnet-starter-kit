using Accounting.Application.Bills.Get.v1;

namespace Accounting.Application.Bills.Search.v1;

/// <summary>
/// Handler for searching bills with filters and pagination.
/// </summary>
public sealed class SearchBillsHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> repository)
    : IRequestHandler<SearchBillsRequest, PagedList<BillResponse>>
{
    public async Task<PagedList<BillResponse>> Handle(SearchBillsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBillsSpec(request);
        var bills = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var billResponses = bills.Adapt<List<BillResponse>>();

        return new PagedList<BillResponse>(
            billResponses,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}

