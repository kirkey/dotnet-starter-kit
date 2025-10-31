using Accounting.Application.AccountsPayableAccounts.Queries;
using Accounting.Application.AccountsPayableAccounts.Responses;

namespace Accounting.Application.AccountsPayableAccounts.Search.v1;

/// <summary>
/// Handler for searching accounts payable accounts with filters.
/// </summary>
public sealed class SearchAPAccountsHandler(
    ILogger<SearchAPAccountsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<AccountsPayableAccount> repository)
    : IRequestHandler<SearchAPAccountsRequest, List<APAccountResponse>>
{
    public async Task<List<APAccountResponse>> Handle(SearchAPAccountsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new AccountsPayableAccountSearchSpec(request.AccountNumber);
        var accounts = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} AP accounts", accounts.Count);

        return accounts.Select(account => new APAccountResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            AccountType = "AccountsPayable",
            IsActive = account.IsActive,
            CurrentBalance = account.CurrentBalance,
            Description = account.Description
        }).ToList();
    }
}
