using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Search.v1;

/// <summary>
/// Request to search leave types with filtering and pagination.
/// </summary>
public class SearchLeaveTypesRequest : PaginationFilter, IRequest<PagedList<LeaveTypeResponse>>
{
    public string? SearchString { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPaid { get; set; }
}

