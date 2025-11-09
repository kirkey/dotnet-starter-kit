using Accounting.Application.AccountsReceivableAccounts.Queries;
using Accounting.Application.AccountsReceivableAccounts.Responses;

namespace Accounting.Application.AccountsReceivableAccounts.Get;

/// <summary>
/// Handler for retrieving an accounts receivable account by ID.
/// </summary>
public sealed class GetArAccountHandler(
    ILogger<GetArAccountHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<AccountsReceivableAccount> repository)
    : IRequestHandler<GetArAccountRequest, ArAccountResponse>
{
    public async Task<ArAccountResponse> Handle(GetArAccountRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.FirstOrDefaultAsync(
            new AccountsReceivableAccountByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (account == null)
        {
            throw new NotFoundException($"AR Account with ID {request.Id} was not found.");
        }

        logger.LogInformation("Retrieved AR account {ArAccountId}", account.Id);

        return new ArAccountResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            IsActive = account.IsActive
        };
    }
}

