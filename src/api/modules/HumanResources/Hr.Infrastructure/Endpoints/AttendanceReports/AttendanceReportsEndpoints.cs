namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.AttendanceReports;

using v1;

/// <summary>
/// Attendance Reports endpoints coordinator.
/// </summary>
public static class AttendanceReportsEndpoints
{
    /// <summary>
    /// Maps all attendance report endpoints.
    /// </summary>
    public static void MapAttendanceReportsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("attendance-reports").WithTags("Attendance Reports");
        group.MapGenerateAttendanceReportEndpoint();
        group.MapGetAttendanceReportEndpoint();
        group.MapSearchAttendanceReportsEndpoint();
        group.MapDownloadAttendanceReportEndpoint();
        group.MapExportAttendanceReportEndpoint();
    }
}

