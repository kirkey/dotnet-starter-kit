using Accounting.Application.ChartOfAccounts.Responses;
using Accounting.Application.ChartOfAccounts.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.ChartOfAccounts.Get.v1;
public sealed class GetChartOfAccountHandler(
    [FromKeyedServices("accounting:accounts")] IReadRepository<ChartOfAccount> repository,
    ICacheService cache)
    : IRequestHandler<GetChartOfAccountQuery, ChartOfAccountResponse>
{
    public async Task<ChartOfAccountResponse> Handle(GetChartOfAccountQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"account:{request.Id}",
            async () =>
            {
                var account = await repository.FirstOrDefaultAsync(
                    new ChartOfAccountByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);
                return account ?? throw new ChartOfAccountNotFoundException(request.Id);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
