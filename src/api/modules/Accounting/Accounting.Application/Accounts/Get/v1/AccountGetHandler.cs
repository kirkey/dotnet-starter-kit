using Accounting.Application.Accounts.Dtos;
using Accounting.Application.Accounts.Queries;
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
    : IRequestHandler<AccountGetRequest, AccountDto>
{
    public async Task<AccountDto> Handle(AccountGetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"account:{request.Id}",
            async () =>
            {
                var account = await repository.SingleOrDefaultAsync(
                    new AccountById(request.Id), cancellationToken).ConfigureAwait(false);
                return account ?? throw new AccountNotFoundException(request.Id);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
