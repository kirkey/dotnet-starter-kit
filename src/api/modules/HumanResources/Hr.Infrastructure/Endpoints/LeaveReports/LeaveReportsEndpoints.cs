namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveReports;

using v1;

/// <summary>
/// Leave Reports endpoints coordinator.
/// </summary>
public static class LeaveReportsEndpoints
{
    /// <summary>
    /// Maps all leave report endpoints.
    /// </summary>
    public static void MapLeaveReportsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("leave-reports").WithTags("Leave Reports");
        group.MapGenerateLeaveReportEndpoint();
        group.MapGetLeaveReportEndpoint();
        group.MapSearchLeaveReportsEndpoint();
        group.MapDownloadLeaveReportEndpoint();
        group.MapExportLeaveReportEndpoint();
    }
}

