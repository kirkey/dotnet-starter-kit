using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using Accounting.Application.Accounts.Get.v1;
using Accounting.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Accounts.Search.v1;
public sealed class AccountSearchHandler(
    [FromKeyedServices("accounting:accounts")] IReadRepository<Account> repository)
    : IRequestHandler<AccountSearchRequest, PagedList<AccountResponse>>
{
    public async Task<PagedList<AccountResponse>> Handle(AccountSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new AccountSearchSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AccountResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
