namespace FSH.Starter.WebApi.HumanResources.Application.HRAnalytics.Export.v1;

/// <summary>
/// Request to export analytics data.
/// </summary>
public sealed record ExportAnalyticsRequest(
    [property: DefaultValue("Excel")] string Format = "Excel", // Excel, PDF, CSV
    [property: DefaultValue(null)] DateTime? FromDate = null,
    [property: DefaultValue(null)] DateTime? ToDate = null,
    [property: DefaultValue(null)] DefaultIdType? DepartmentId = null);

