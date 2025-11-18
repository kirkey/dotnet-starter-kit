using Accounting.Application.AccountsPayableAccounts.Queries;
using Accounting.Application.AccountsPayableAccounts.Responses;

namespace Accounting.Application.AccountsPayableAccounts.Search.v1;

/// <summary>
/// Handler for searching accounts payable accounts with filters and pagination.
/// </summary>
public sealed class SearchApAccountsHandler(
    ILogger<SearchApAccountsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<AccountsPayableAccount> repository)
    : IRequestHandler<SearchApAccountsRequest, PagedList<ApAccountResponse>>
{
    public async Task<PagedList<ApAccountResponse>> Handle(SearchApAccountsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new AccountsPayableAccountSearchSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} of {Total} AP accounts", items.Count, totalCount);

        return new PagedList<ApAccountResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
