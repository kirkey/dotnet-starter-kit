using FSH.Starter.WebApi.HumanResources.Application.Attendance.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Search.v1;

public class SearchAttendanceRequest : PaginationFilter, IRequest<PagedList<AttendanceResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Status { get; set; }
    public bool? IsApproved { get; set; }
    public bool? IsActive { get; set; }
}

