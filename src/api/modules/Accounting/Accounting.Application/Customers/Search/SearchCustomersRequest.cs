using Accounting.Application.Customers.Dtos;
using FSH.Framework.Core.Paging;
using MediatR;

namespace Accounting.Application.Customers.Search;

public class SearchCustomersRequest : PaginationFilter, IRequest<PagedList<CustomerDto>>
{
    public string? CustomerCode { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}


