using System.ComponentModel;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.HRAnalytics.v1;

using Shared.Authorization;

/// <summary>
/// Endpoint for exporting HR analytics data.
/// </summary>
public static class ExportHRAnalyticsEndpoint
{
    /// <summary>
    /// Maps the export HR analytics endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapExportHRAnalyticsEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/export", async (ExportAnalyticsRequest request, ISender mediator) =>
        {
            // TODO: Implement export logic for Excel/PDF/CSV
            return Results.Ok(new { message = "Analytics export functionality to be implemented" });
        })
        .WithName(nameof(ExportHRAnalyticsEndpoint))
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

/// <summary>
/// Request to export analytics data.
/// </summary>
public record ExportAnalyticsRequest(
    [property: DefaultValue("Excel")] string Format = "Excel", // Excel, PDF, CSV
    [property: DefaultValue(null)] DateTime? FromDate = null,
    [property: DefaultValue(null)] DateTime? ToDate = null,
    [property: DefaultValue(null)] DefaultIdType? DepartmentId = null);

