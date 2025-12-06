using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Search.v1;

public sealed class SearchMobileWalletsHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IReadRepository<MobileWallet> repository)
    : IRequestHandler<SearchMobileWalletsCommand, PagedList<MobileWalletResponse>>
{
    public async Task<PagedList<MobileWalletResponse>> Handle(SearchMobileWalletsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchMobileWalletsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<MobileWalletResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
