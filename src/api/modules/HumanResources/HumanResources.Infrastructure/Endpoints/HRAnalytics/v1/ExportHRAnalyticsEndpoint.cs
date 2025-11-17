using System.ComponentModel;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.HRAnalytics.v1;

using FSH.Starter.WebApi.HumanResources.Application.HRAnalytics.Export.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for exporting HR analytics data.
/// </summary>
public static class ExportHrAnalyticsEndpoint
{
    /// <summary>
    /// Maps the export HR analytics endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapExportHrAnalyticsEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/export", async (ExportAnalyticsRequest request, ISender mediator) =>
        {
            // TODO: Implement export logic for Excel/PDF/CSV
            return Results.Ok(new { message = "Analytics export functionality to be implemented" });
        })
        .WithName(nameof(ExportHrAnalyticsEndpoint))
        .WithSummary("Export HR analytics")
        .WithDescription("Exports HR analytics data in specified format (Excel/PDF/CSV)")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Analytics))
        .MapToApiVersion(1);
    }
}


