using FSH.Starter.WebApi.HumanResources.Application.Attendances.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Attendances.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Search.v1;

/// <summary>
/// Handler for searching attendance records.
/// </summary>
public sealed class SearchAttendanceHandler(
    [FromKeyedServices("hr:attendance")] IReadRepository<Attendance> repository)
    : IRequestHandler<SearchAttendanceRequest, PagedList<AttendanceResponse>>
{
    /// <summary>
    /// Handles the request to search attendance records.
    /// </summary>
    public async Task<PagedList<AttendanceResponse>> Handle(
        SearchAttendanceRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchAttendanceSpec(request);
        var records = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = records.Select(record => new AttendanceResponse
        {
            Id = record.Id,
            EmployeeId = record.EmployeeId,
            AttendanceDate = record.AttendanceDate,
            ClockInTime = record.ClockInTime,
            ClockOutTime = record.ClockOutTime,
            ClockInLocation = record.ClockInLocation,
            ClockOutLocation = record.ClockOutLocation,
            HoursWorked = record.HoursWorked,
            Status = record.Status,
            MinutesLate = record.MinutesLate,
            Reason = record.Reason,
            IsApproved = record.IsApproved,
            ManagerComment = record.ManagerComment,
            IsActive = record.IsActive
        }).ToList();

        return new PagedList<AttendanceResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}
