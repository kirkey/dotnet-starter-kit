namespace FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Export.v1;

/// <summary>
/// Request to export a payroll report.
/// </summary>
public sealed record ExportPayrollReportRequest(
    [property: DefaultValue("Excel")] string Format = "Excel", // Excel, PDF, CSV
    [property: DefaultValue(false)] bool IncludeDetails = false);

