using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Search.v1;

public class SearchShareProductsCommand : PaginationFilter, IRequest<PagedList<ShareProductResponse>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public bool? PaysDividends { get; set; }
    public bool? AllowTransfer { get; set; }
}
