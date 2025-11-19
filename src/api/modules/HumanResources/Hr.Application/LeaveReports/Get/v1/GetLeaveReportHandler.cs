using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Get.v1;

/// <summary>
/// Handler for retrieving leave report details.
/// </summary>
public sealed class GetLeaveReportHandler(
    ILogger<GetLeaveReportHandler> logger,
    [FromKeyedServices("hr:leavereports")] IReadRepository<LeaveReport> repository)
    : IRequestHandler<GetLeaveReportRequest, LeaveReportResponse>
{
    /// <summary>
    /// Handles the get leave report query.
    /// </summary>
    public async Task<LeaveReportResponse> Handle(
        GetLeaveReportRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Leave report with ID {request.Id} not found");

        logger.LogInformation(
            "Retrieved leave report: {ReportId}, Type: {ReportType}",
            report.Id,
            report.ReportType);

        return new LeaveReportResponse(
            Id: report.Id,
            ReportType: report.ReportType,
            Title: report.Title,
            FromDate: report.FromDate,
            ToDate: report.ToDate,
            GeneratedOn: report.GeneratedOn,
            DepartmentId: report.DepartmentId,
            EmployeeId: report.EmployeeId,
            TotalEmployees: report.TotalEmployees,
            TotalLeaveTypes: report.TotalLeaveTypes,
            TotalLeaveRequests: report.TotalLeaveRequests,
            ApprovedLeaveCount: report.ApprovedLeaveCount,
            PendingLeaveCount: report.PendingLeaveCount,
            RejectedLeaveCount: report.RejectedLeaveCount,
            TotalLeaveConsumed: report.TotalLeaveConsumed,
            AverageLeavePerEmployee: report.AverageLeavePerEmployee,
            ExportPath: report.ExportPath,
            Notes: report.Notes,
            IsActive: report.IsActive,
            CreatedOn: report.CreatedOn.DateTime,
            CreatedBy: report.CreatedBy,
            LastModifiedOn: report.LastModifiedOn,
            LastModifiedBy: report.LastModifiedBy);
    }
}

