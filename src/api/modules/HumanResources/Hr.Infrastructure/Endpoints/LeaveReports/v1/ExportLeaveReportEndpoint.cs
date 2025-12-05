namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Export.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for exporting leave reports.
/// </summary>
public static class ExportLeaveReportEndpoint
{
    /// <summary>
    /// Maps the export leave report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapExportLeaveReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/{id}/export", async (DefaultIdType id, ExportLeaveReportRequest request, ISender mediator) =>
        {
            return Results.Ok(new { message = "Report export functionality to be implemented" });
        })
        .WithName(nameof(ExportLeaveReportEndpoint))
        .WithSummary("Export leave report")
        .WithDescription("Exports a leave report in specified format")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
        .MapToApiVersion(1);
    }
}


