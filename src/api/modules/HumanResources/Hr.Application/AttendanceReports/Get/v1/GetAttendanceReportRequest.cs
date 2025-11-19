namespace FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Get.v1;

/// <summary>
/// Query to retrieve an attendance report by ID.
/// </summary>
public sealed record GetAttendanceReportRequest(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id) 
    : IRequest<AttendanceReportResponse>;

/// <summary>
/// Response object for attendance report details.
/// </summary>
public sealed record AttendanceReportResponse(
    DefaultIdType Id,
    string ReportType,
    string Title,
    DateTime FromDate,
    DateTime ToDate,
    DateTime GeneratedOn,
    DefaultIdType? DepartmentId,
    DefaultIdType? EmployeeId,
    int TotalWorkingDays,
    int TotalEmployees,
    int PresentCount,
    int AbsentCount,
    int LateCount,
    int HalfDayCount,
    int OnLeaveCount,
    decimal AttendancePercentage,
    decimal LatePercentage,
    string? ExportPath,
    string? Notes,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DefaultIdType? CreatedBy,
    DateTimeOffset? LastModifiedOn,
    DefaultIdType? LastModifiedBy);

