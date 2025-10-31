using Accounting.Application.AccountsReceivableAccounts.Queries;
using Accounting.Application.AccountsReceivableAccounts.Responses;

namespace Accounting.Application.AccountsReceivableAccounts.Get;

/// <summary>
/// Handler for retrieving an accounts receivable account by ID.
/// </summary>
public sealed class GetARAccountHandler(
    ILogger<GetARAccountHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<AccountsReceivableAccount> repository)
    : IRequestHandler<GetARAccountRequest, ARAccountResponse>
{
    public async Task<ARAccountResponse> Handle(GetARAccountRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.FirstOrDefaultAsync(
            new AccountsReceivableAccountByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (account == null)
        {
            throw new NotFoundException($"AR Account with ID {request.Id} was not found.");
        }

        logger.LogInformation("Retrieved AR account {ARAccountId}", account.Id);

        return new ARAccountResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            IsActive = account.IsActive
        };
    }
}

