using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Get.v1;

/// <summary>
/// Handler for retrieving attendance report details.
/// </summary>
public sealed class GetAttendanceReportHandler(
    ILogger<GetAttendanceReportHandler> logger,
    [FromKeyedServices("hr:attendancereports")] IReadRepository<AttendanceReport> repository)
    : IRequestHandler<GetAttendanceReportRequest, AttendanceReportResponse>
{
    /// <summary>
    /// Handles the get attendance report query.
    /// </summary>
    public async Task<AttendanceReportResponse> Handle(
        GetAttendanceReportRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Attendance report with ID {request.Id} not found");

        logger.LogInformation(
            "Retrieved attendance report: {ReportId}, Type: {ReportType}",
            report.Id,
            report.ReportType);

        return new AttendanceReportResponse(
            Id: report.Id,
            ReportType: report.ReportType,
            Title: report.Title,
            FromDate: report.FromDate,
            ToDate: report.ToDate,
            GeneratedOn: report.GeneratedOn,
            DepartmentId: report.DepartmentId,
            EmployeeId: report.EmployeeId,
            TotalWorkingDays: report.TotalWorkingDays,
            TotalEmployees: report.TotalEmployees,
            PresentCount: report.PresentCount,
            AbsentCount: report.AbsentCount,
            LateCount: report.LateCount,
            HalfDayCount: report.HalfDayCount,
            OnLeaveCount: report.OnLeaveCount,
            AttendancePercentage: report.AttendancePercentage,
            LatePercentage: report.LatePercentage,
            ExportPath: report.ExportPath,
            Notes: report.Notes,
            IsActive: report.IsActive,
            CreatedOn: report.CreatedOn,
            CreatedBy: report.CreatedBy,
            LastModifiedOn: report.LastModifiedOn,
            LastModifiedBy: report.LastModifiedBy);
    }
}

