using Accounting.Application.DepreciationMethods.Responses;

namespace Accounting.Application.DepreciationMethods.Search.v1;

public sealed class SearchDepreciationMethodsRequest : PaginationFilter, IRequest<PagedList<DepreciationMethodResponse>>
{
    public string? MethodCode { get; init; }
    public string? MethodName { get; init; }
    public bool? IsActive { get; init; }
}
