using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Search.v1;

public class SearchFeeDefinitionsCommand : PaginationFilter, IRequest<PagedList<FeeDefinitionResponse>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? FeeType { get; set; }
    public string? CalculationType { get; set; }
    public string? AppliesTo { get; set; }
    public bool? IsActive { get; set; }
}
