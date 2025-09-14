using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Application.ChartOfAccounts.Queries;

namespace Accounting.Application.ChartOfAccounts.Get.v1;
public sealed class ChartOfAccountGetHandler(
    [FromKeyedServices("accounting:accounts")] IReadRepository<ChartOfAccount> repository,
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
                    new ChartOfAccountByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);
                return account ?? throw new ChartOfAccountNotFoundException(request.Id);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
