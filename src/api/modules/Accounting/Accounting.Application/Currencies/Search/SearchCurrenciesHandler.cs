using Accounting.Application.Currencies.Dtos;
using Accounting.Domain;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Currencies.Search;

public sealed class SearchCurrenciesHandler(
    [FromKeyedServices("accounting:currencies")] IReadRepository<Currency> repository)
    : IRequestHandler<SearchCurrenciesRequest, PagedList<CurrencyDto>>
{
    public async Task<PagedList<CurrencyDto>> Handle(SearchCurrenciesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCurrenciesSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CurrencyDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}


