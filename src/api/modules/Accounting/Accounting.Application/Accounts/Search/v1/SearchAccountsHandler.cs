using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using Accounting.Application.Accounts.Get.v1;
using Accounting.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Accounts.Search.v1;
public sealed class SearchAccountsHandler(
    [FromKeyedServices("accounting:accounts")] IReadRepository<Account> repository)
    : IRequestHandler<SearchAccountsCommand, PagedList<AccountResponse>>
{
    public async Task<PagedList<AccountResponse>> Handle(SearchAccountsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAccountSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AccountResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
