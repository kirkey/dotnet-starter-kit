namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.AttendanceReports.v1;

using Shared.Authorization;

/// <summary>
/// Endpoint for downloading an attendance report.
/// </summary>
public static class DownloadAttendanceReportEndpoint
{
    /// <summary>
    /// Maps the download attendance report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapDownloadAttendanceReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/{id}/download", (DefaultIdType id, ISender mediator) =>
        {
            // TODO: Implement report download logic
            // Return file stream with report data
            return Results.Ok(new { message = "Report download functionality to be implemented" });
        })
        .WithName(nameof(DownloadAttendanceReportEndpoint))
        .WithSummary("Download attendance report")
        .WithDescription("Downloads an attendance report in specified format (PDF/Excel)")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
        .MapToApiVersion(1);
    }
}

