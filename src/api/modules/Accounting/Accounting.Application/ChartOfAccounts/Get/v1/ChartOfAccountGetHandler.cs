using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Application.ChartOfAccounts.Queries;
using Accounting.Domain;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.ChartOfAccounts.Get.v1;
public sealed class ChartOfAccountGetHandler(
    [FromKeyedServices("accounting:ChartOfAccounts")] IReadRepository<ChartOfAccount> repository,
    ICacheService cache)
    : IRequestHandler<ChartOfAccountGetRequest, ChartOfAccountDto>
{
    public async Task<ChartOfAccountDto> Handle(ChartOfAccountGetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"account:{request.Id}",
            async () =>
            {
                var account = await repository.SingleOrDefaultAsync(
                    new ChartOfAccountById(request.Id), cancellationToken).ConfigureAwait(false);
                return account ?? throw new ChartOfAccountNotFoundException(request.Id);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
