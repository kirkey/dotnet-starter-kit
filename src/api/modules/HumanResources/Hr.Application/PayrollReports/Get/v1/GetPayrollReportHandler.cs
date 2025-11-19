using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Get.v1;

/// <summary>
/// Handler for retrieving payroll report details.
/// </summary>
public sealed class GetPayrollReportHandler(
    ILogger<GetPayrollReportHandler> logger,
    [FromKeyedServices("hr:payrollreports")] IReadRepository<PayrollReport> repository)
    : IRequestHandler<GetPayrollReportRequest, PayrollReportResponse>
{
    /// <summary>
    /// Handles the get payroll report query.
    /// </summary>
    public async Task<PayrollReportResponse> Handle(
        GetPayrollReportRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Payroll report with ID {request.Id} not found");

        logger.LogInformation(
            "Retrieved payroll report: {ReportId}, Type: {ReportType}",
            report.Id,
            report.ReportType);

        return new PayrollReportResponse(
            Id: report.Id,
            ReportType: report.ReportType,
            Title: report.Title,
            FromDate: report.FromDate,
            ToDate: report.ToDate,
            GeneratedOn: report.GeneratedOn,
            DepartmentId: report.DepartmentId,
            EmployeeId: report.EmployeeId,
            PayrollPeriod: report.PayrollPeriod,
            TotalEmployees: report.TotalEmployees,
            TotalPayrollRuns: report.TotalPayrollRuns,
            TotalGrossPay: report.TotalGrossPay,
            TotalNetPay: report.TotalNetPay,
            TotalDeductions: report.TotalDeductions,
            TotalTaxes: report.TotalTaxes,
            TotalBenefits: report.TotalBenefits,
            AverageGrossPerEmployee: report.AverageGrossPerEmployee,
            AverageNetPerEmployee: report.AverageNetPerEmployee,
            ExportPath: report.ExportPath,
            Notes: report.Notes,
            IsActive: report.IsActive,
            CreatedOn: report.CreatedOn,
            CreatedBy: report.CreatedBy,
            LastModifiedOn: report.LastModifiedOn,
            LastModifiedBy: report.LastModifiedBy);
    }
}

