using Accounting.Application.Currencies.Dtos;
using FSH.Framework.Core.Paging;
using MediatR;

namespace Accounting.Application.Currencies.Search;

public class SearchCurrenciesRequest : PaginationFilter, IRequest<PagedList<CurrencyDto>>
{
    public string? CurrencyCode { get; set; }
    public string? Name { get; set; }
}


