using FSH.Framework.Core.Paging;
using MediatR;

namespace Accounting.Application.Vendors.Search.v1;

public class VendorSearchQuery : PaginationFilter, IRequest<PagedList<VendorSearchResponse>>
{
    // public string? Name { get; set; }
}
