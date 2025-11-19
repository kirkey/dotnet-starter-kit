namespace FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Generate.v1;

/// <summary>
/// Command to generate a new attendance report.
/// </summary>
public sealed record GenerateAttendanceReportCommand(
    [property: DefaultValue("Summary")] string ReportType,
    [property: DefaultValue("Attendance Report")] string Title,
    [property: DefaultValue(null)] DateTime? FromDate = null,
    [property: DefaultValue(null)] DateTime? ToDate = null,
    [property: DefaultValue(null)] DefaultIdType? DepartmentId = null,
    [property: DefaultValue(null)] DefaultIdType? EmployeeId = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<GenerateAttendanceReportResponse>;

/// <summary>
/// Response for attendance report generation.
/// </summary>
public record GenerateAttendanceReportResponse(
    DefaultIdType ReportId,
    string ReportType,
    string Title,
    DateTime GeneratedOn,
    int TotalWorkingDays,
    int TotalEmployees,
    int PresentCount,
    int AbsentCount,
    decimal AttendancePercentage);

