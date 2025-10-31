using Accounting.Application.AccountsReceivableAccounts.Queries;
using Accounting.Application.AccountsReceivableAccounts.Responses;

namespace Accounting.Application.AccountsReceivableAccounts.Search.v1;

/// <summary>
/// Handler for searching accounts receivable accounts with filters.
/// </summary>
public sealed class SearchARAccountsHandler(
    ILogger<SearchARAccountsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<AccountsReceivableAccount> repository)
    : IRequestHandler<SearchARAccountsRequest, List<ARAccountResponse>>
{
    public async Task<List<ARAccountResponse>> Handle(SearchARAccountsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new AccountsReceivableAccountSearchSpec(request.AccountNumber);
        var accounts = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} AR accounts", accounts.Count);

        return accounts.Select(account => new ARAccountResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            IsActive = account.IsActive
        }).ToList();
    }
}

