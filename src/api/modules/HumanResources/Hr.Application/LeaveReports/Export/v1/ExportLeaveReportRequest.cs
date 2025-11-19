namespace FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Export.v1;

/// <summary>
/// Request to export a leave report.
/// </summary>
public sealed record ExportLeaveReportRequest(
    [property: DefaultValue("Excel")] string Format = "Excel",
    [property: DefaultValue(null)] bool? IncludeDetails = null);

