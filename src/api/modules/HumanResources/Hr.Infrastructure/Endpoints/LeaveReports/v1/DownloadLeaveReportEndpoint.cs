namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveReports.v1;

using Shared.Authorization;

/// <summary>
/// Endpoint for downloading a leave report.
/// </summary>
public static class DownloadLeaveReportEndpoint
{
    /// <summary>
    /// Maps the download leave report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapDownloadLeaveReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/{id}/download", async (DefaultIdType id, ISender mediator) =>
        {
            return Results.Ok(new { message = "Report download functionality to be implemented" });
        })
        .WithName(nameof(DownloadLeaveReportEndpoint))
        .WithSummary("Download leave report")
        .WithDescription("Downloads a leave report in specified format (PDF/Excel)")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
        .MapToApiVersion(1);
    }
}

