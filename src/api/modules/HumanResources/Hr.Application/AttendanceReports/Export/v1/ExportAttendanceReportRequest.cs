namespace FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Export.v1;

/// <summary>
/// Request to export an attendance report.
/// </summary>
public sealed record ExportAttendanceReportRequest(
    [property: DefaultValue("Excel")] string Format = "Excel", // Excel, CSV, PDF, JSON
    [property: DefaultValue(null)] bool? IncludeDetails = null);

