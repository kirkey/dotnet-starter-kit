namespace FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Get.v1;

/// <summary>
/// Query to retrieve a leave report by ID.
/// </summary>
public sealed record GetLeaveReportRequest(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id) 
    : IRequest<LeaveReportResponse>;

/// <summary>
/// Response object for leave report details.
/// </summary>
public sealed record LeaveReportResponse(
    DefaultIdType Id,
    string ReportType,
    string Title,
    DateTime FromDate,
    DateTime ToDate,
    DateTime GeneratedOn,
    DefaultIdType? DepartmentId,
    DefaultIdType? EmployeeId,
    int TotalEmployees,
    int TotalLeaveTypes,
    int TotalLeaveRequests,
    int ApprovedLeaveCount,
    int PendingLeaveCount,
    int RejectedLeaveCount,
    decimal TotalLeaveConsumed,
    decimal AverageLeavePerEmployee,
    string? ExportPath,
    string? Notes,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DefaultIdType? CreatedBy,
    DateTimeOffset? LastModifiedOn,
    DefaultIdType? LastModifiedBy);

