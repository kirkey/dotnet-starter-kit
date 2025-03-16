using Microsoft.Extensions.DependencyInjection;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Caching;
using Accounting.Domain;
using MediatR;

namespace Accounting.Application.Accounts.Get.v1;
public sealed class AccountGetHandler(
    [FromKeyedServices("accounting:accounts")] IReadRepository<Account> repository,
    ICacheService cache)
    : IRequestHandler<AccountGetRequest, AccountResponse>
{
    public async Task<AccountResponse> Handle(AccountGetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"account:{request.Id}",
            async () =>
            {
                var account = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return account == null
                    ? throw new AccountNotFoundException(request.Id)
                    : new AccountResponse(
                            account.Id, account.Name,
                            account.AccountCategory, account.Type, account.ParentCode, account.Code, account.Balance,
                            account.Remarks, account.Status, account.Description, account.Notes, account.FilePath
                        );
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}
