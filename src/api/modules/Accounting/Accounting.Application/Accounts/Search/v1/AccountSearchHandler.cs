using Accounting.Application.Accounts.Dtos;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using Accounting.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Accounts.Search.v1;
public sealed class AccountSearchHandler(
    [FromKeyedServices("accounting:accounts")] IReadRepository<Account> repository)
    : IRequestHandler<AccountSearchRequest, PagedList<AccountDto>>
{
    public async Task<PagedList<AccountDto>> Handle(AccountSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new AccountSearchSpec(request);

        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AccountDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
