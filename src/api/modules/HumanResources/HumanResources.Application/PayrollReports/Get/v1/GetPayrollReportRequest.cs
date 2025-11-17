namespace FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Get.v1;

/// <summary>
/// Query to retrieve a payroll report by ID.
/// </summary>
public sealed record GetPayrollReportRequest(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id) 
    : IRequest<PayrollReportResponse>;

/// <summary>
/// Response object for payroll report details.
/// </summary>
public sealed record PayrollReportResponse(
    DefaultIdType Id,
    string ReportType,
    string Title,
    DateTime FromDate,
    DateTime ToDate,
    DateTime GeneratedOn,
    DefaultIdType? DepartmentId,
    DefaultIdType? EmployeeId,
    string? PayrollPeriod,
    int TotalEmployees,
    int TotalPayrollRuns,
    decimal TotalGrossPay,
    decimal TotalNetPay,
    decimal TotalDeductions,
    decimal TotalTaxes,
    decimal TotalBenefits,
    decimal AverageGrossPerEmployee,
    decimal AverageNetPerEmployee,
    string? ExportPath,
    string? Notes,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DefaultIdType? CreatedBy,
    DateTimeOffset? LastModifiedOn,
    DefaultIdType? LastModifiedBy);

