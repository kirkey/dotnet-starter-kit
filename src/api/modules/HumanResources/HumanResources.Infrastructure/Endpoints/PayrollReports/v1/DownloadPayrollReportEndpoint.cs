namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollReports.v1;

using Shared.Authorization;

/// <summary>
/// Endpoint for downloading a payroll report.
/// </summary>
public static class DownloadPayrollReportEndpoint
{
    /// <summary>
    /// Maps the download payroll report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapDownloadPayrollReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/{id}/download", async (DefaultIdType id, ISender mediator) =>
        {
            // TODO: Implement report download logic
            // Return file stream with report data
            return Results.Ok(new { message = "Report download functionality to be implemented" });
        })
        .WithName(nameof(DownloadPayrollReportEndpoint))
        .WithSummary("Download payroll report")
        .WithDescription("Downloads a payroll report in specified format (PDF/Excel)")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}

