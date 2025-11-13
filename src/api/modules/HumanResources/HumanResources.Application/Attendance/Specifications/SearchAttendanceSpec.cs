using FSH.Starter.WebApi.HumanResources.Application.Attendance.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Attendance.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Specifications;

public class SearchAttendanceSpec : EntitiesByPaginationFilterSpec<Domain.Entities.Attendance, AttendanceResponse>
{
    public SearchAttendanceSpec(SearchAttendanceRequest request)
        : base(request) =>
        Query
            .Where(a => a.EmployeeId == request.EmployeeId, request.EmployeeId.HasValue)
            .Where(a => a.AttendanceDate >= request.FromDate, request.FromDate.HasValue)
            .Where(a => a.AttendanceDate <= request.ToDate, request.ToDate.HasValue)
            .Where(a => a.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(a => a.IsApproved == request.IsApproved, request.IsApproved.HasValue)
            .Where(a => a.IsActive == request.IsActive, request.IsActive.HasValue)
            .OrderByDescending(a => a.AttendanceDate, !request.HasOrderBy());
}

