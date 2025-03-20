using Accounting.Application.Accounts.Exceptions;
using Accounting.Application.Accounts.Queries;
using FSH.Framework.Core.Persistence;
using Accounting.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Accounts.Create.v1;
public sealed class AccountCreateRequestHandler(
    ILogger<AccountCreateRequestHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<Account> repository)
    : IRequestHandler<AccountCreateRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(AccountCreateRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate account code
        var existingAccount = await repository.FirstOrDefaultAsync(new AccountByCode(request.Code), cancellationToken)
            .ConfigureAwait(false);
        if (existingAccount is not null)
            throw new AccountForbiddenException(request.Code);

        var account = Account.Create(request.AccountCategory, request.AccountType, request.ParentCode, request.Code, request.Name, request.Balance,
            request.Description, request.Notes);

        await repository.AddAsync(account, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("account created {AccountId}", account.Id);

        return account.Id;
    }
}
