using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Search.v1;

public sealed class SearchShareAccountsHandler(
    [FromKeyedServices("microfinance:shareaccounts")] IReadRepository<ShareAccount> repository)
    : IRequestHandler<SearchShareAccountsCommand, PagedList<ShareAccountResponse>>
{
    public async Task<PagedList<ShareAccountResponse>> Handle(SearchShareAccountsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchShareAccountsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ShareAccountResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
