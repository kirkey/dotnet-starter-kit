using FSH.Starter.WebApi.HumanResources.Application.Attendance.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Search.v1;

public class SearchAttendanceRequest : PaginationFilter, IRequest<PagedList<AttendanceResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }
    public bool? IsApproved { get; set; }
}

