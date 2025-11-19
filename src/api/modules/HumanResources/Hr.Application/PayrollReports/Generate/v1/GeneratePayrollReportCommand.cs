namespace FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Generate.v1;

/// <summary>
/// Command to generate a new payroll report.
/// </summary>
public sealed record GeneratePayrollReportCommand(
    [property: DefaultValue("Summary")] string ReportType,
    [property: DefaultValue("Payroll Report")] string Title,
    [property: DefaultValue(null)] DateTime? FromDate = null,
    [property: DefaultValue(null)] DateTime? ToDate = null,
    [property: DefaultValue(null)] DefaultIdType? DepartmentId = null,
    [property: DefaultValue(null)] DefaultIdType? EmployeeId = null,
    [property: DefaultValue(null)] string? PayrollPeriod = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<GeneratePayrollReportResponse>;

/// <summary>
/// Response for payroll report generation.
/// </summary>
public record GeneratePayrollReportResponse(
    DefaultIdType ReportId,
    string ReportType,
    string Title,
    DateTime GeneratedOn,
    int TotalEmployees,
    int TotalPayrollRuns,
    decimal TotalGrossPay,
    decimal TotalNetPay,
    decimal TotalDeductions);

