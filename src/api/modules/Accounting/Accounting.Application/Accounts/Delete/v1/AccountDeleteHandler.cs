using FSH.Framework.Core.Persistence;
using Accounting.Domain;
using Accounting.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Accounts.Delete.v1;
public sealed class AccountDeleteHandler(
    ILogger<AccountDeleteHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<Account> repository)
    : IRequestHandler<AccountDeleteRequest>
{
    public async Task Handle(AccountDeleteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var account = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = account ?? throw new AccountNotFoundException(request.Id);
        await repository.DeleteAsync(account, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("account with id: {AccountId} deleted", account.Id);
    }
}
