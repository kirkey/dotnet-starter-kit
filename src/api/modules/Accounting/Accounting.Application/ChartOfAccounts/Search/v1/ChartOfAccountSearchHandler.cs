using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Domain;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.ChartOfAccounts.Search.v1;

public sealed class ChartOfAccountSearchHandler(
    [FromKeyedServices("accounting:ChartOfAccounts")] IReadRepository<ChartOfAccount> repository)
    : IRequestHandler<ChartOfAccountSearchRequest, PagedList<ChartOfAccountDto>>
{
    public async Task<PagedList<ChartOfAccountDto>> Handle(ChartOfAccountSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new ChartOfAccountSearchSpec(request);

        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ChartOfAccountDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
