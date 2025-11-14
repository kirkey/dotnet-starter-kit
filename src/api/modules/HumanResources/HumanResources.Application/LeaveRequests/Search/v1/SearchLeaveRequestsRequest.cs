using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Search.v1;

/// <summary>
/// Request to search leave requests with filtering and pagination.
/// </summary>
public class SearchLeaveRequestsRequest : PaginationFilter, IRequest<PagedList<LeaveRequestResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }
    public DefaultIdType? LeaveTypeId { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

