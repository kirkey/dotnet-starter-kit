using FSH.Framework.Core.Persistence;
using Accounting.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Accounts.Create.v1;
public sealed class AccountCreateRequestHandler(
    ILogger<AccountCreateRequestHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<Account> repository)
    : IRequestHandler<AccountCreateRequest, AccountCreateRequestResponse>
{
    public async Task<AccountCreateRequestResponse> Handle(AccountCreateRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = Account.Create(request.AccountCategory, request.Type, request.ParentCode, request.Code, request.Name, request.Balance,
            request.Description, request.Notes);

        await repository.AddAsync(account, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("account created {AccountId}", account.Id);

        return new AccountCreateRequestResponse(account.Id);
    }
}
