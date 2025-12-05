using FSH.Starter.WebApi.HumanResources.Application.HRAnalytics.Export.v1;
using FSH.Starter.WebApi.HumanResources.Application.HRAnalytics.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.HRAnalytics;

/// <summary>
/// Endpoint routes for HR Analytics.
/// </summary>
public class HrAnalyticsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all HR analytics endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/analytics").WithTags("hr-analytics");

        group.MapGet("/", async (
            DateTime? fromDate,
            DateTime? toDate,
            DefaultIdType? departmentId,
            ISender mediator) =>
            {
                var request = new GetHrAnalyticsRequest(fromDate, toDate, departmentId);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetHrAnalytics")
            .WithSummary("Get HR analytics")
            .WithDescription("Retrieves comprehensive HR metrics and KPIs including headcount, attendance, payroll, performance, and more")
            .Produces<HrAnalyticsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Analytics))
            .MapToApiVersion(1);

        group.MapGet("/department/{departmentId}", async (
            DefaultIdType departmentId,
            DateTime? fromDate,
            DateTime? toDate,
            ISender mediator) =>
            {
                var request = new GetHrAnalyticsRequest(fromDate, toDate, departmentId);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetDepartmentAnalytics")
            .WithSummary("Get department analytics")
            .WithDescription("Retrieves HR metrics and KPIs filtered by specific department")
            .Produces<HrAnalyticsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Analytics))
            .MapToApiVersion(1);

        group.MapPost("/export", async (ExportAnalyticsRequest request, ISender mediator) =>
            {
                // TODO: Implement export logic for Excel/PDF/CSV
                return Results.Ok(new { message = "Analytics export functionality to be implemented" });
            })
            .WithName("ExportHrAnalytics")
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

