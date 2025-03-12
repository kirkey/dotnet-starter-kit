using Microsoft.Extensions.DependencyInjection;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Caching;
using Accounting.Domain;
using MediatR;

namespace Accounting.Application.Accounts.Get.v1;
public sealed class GetAccountHandler(
    [FromKeyedServices("accounting:accounts")] IReadRepository<Account> repository,
    ICacheService cache)
    : IRequestHandler<GetAccountRequest, AccountResponse>
{
    public async Task<AccountResponse> Handle(GetAccountRequest request, CancellationToken cancellationToken)
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
                            account.Category, account.TransactionType, account.ParentCode, account.Code, account.Balance,
                            account.Remarks, account.Status.ToString(), account.Description, account.Notes, account.FilePath
                        );
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}
