using Accounting.Application.AccountsPayableAccounts.Queries;
using Accounting.Application.AccountsPayableAccounts.Responses;

namespace Accounting.Application.AccountsPayableAccounts.Get;

/// <summary>
/// Handler for retrieving an accounts payable account by ID.
/// </summary>
public sealed class GetApAccountHandler(
    ILogger<GetApAccountHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<AccountsPayableAccount> repository)
    : IRequestHandler<GetApAccountRequest, ApAccountResponse>
{
    public async Task<ApAccountResponse> Handle(GetApAccountRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.FirstOrDefaultAsync(
            new AccountsPayableAccountByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (account == null)
        {
            throw new NotFoundException($"AP Account with ID {request.Id} was not found.");
        }

        logger.LogInformation("Retrieved AP account {APAccountId}", account.Id);

        return new ApAccountResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            AccountType = "AccountsPayable",
            IsActive = account.IsActive,
            CurrentBalance = account.CurrentBalance,
            Description = account.Description
        };
    }
}

