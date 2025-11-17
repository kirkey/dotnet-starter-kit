namespace FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Generate.v1;

/// <summary>
/// Command to generate a new leave report.
/// </summary>
public sealed record GenerateLeaveReportCommand(
    [property: DefaultValue("Summary")] string ReportType,
    [property: DefaultValue("Leave Report")] string Title,
    [property: DefaultValue(null)] DateTime? FromDate = null,
    [property: DefaultValue(null)] DateTime? ToDate = null,
    [property: DefaultValue(null)] DefaultIdType? DepartmentId = null,
    [property: DefaultValue(null)] DefaultIdType? EmployeeId = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<GenerateLeaveReportResponse>;

/// <summary>
/// Response for leave report generation.
/// </summary>
public record GenerateLeaveReportResponse(
    DefaultIdType ReportId,
    string ReportType,
    string Title,
    DateTime GeneratedOn,
    int TotalEmployees,
    int TotalLeaveRequests,
    int ApprovedLeaveCount,
    int PendingLeaveCount,
    decimal TotalLeaveConsumed);

