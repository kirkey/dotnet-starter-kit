using Accounting.Application.Currencies.Dtos;
using Accounting.Domain;
using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;

namespace Accounting.Application.Currencies.Search;

public sealed class SearchCurrenciesSpec : EntitiesByPaginationFilterSpec<Currency, CurrencyDto>
{
    public SearchCurrenciesSpec(SearchCurrenciesRequest request) : base(request)
    {
        Query
            .OrderBy(c => c.CurrencyCode, !request.HasOrderBy())
            .Where(c => c.CurrencyCode!.Contains(request.CurrencyCode!), !string.IsNullOrEmpty(request.CurrencyCode))
            .Where(c => c.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name));
    }
}


