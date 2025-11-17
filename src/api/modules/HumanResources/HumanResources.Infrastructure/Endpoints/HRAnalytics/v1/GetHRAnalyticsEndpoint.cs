namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.HRAnalytics.v1;

using FSH.Starter.WebApi.HumanResources.Application.HRAnalytics.Get.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for retrieving company-wide HR analytics.
/// </summary>
public static class GetHRAnalyticsEndpoint
{
    /// <summary>
    /// Maps the get HR analytics endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGetHRAnalyticsEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/", async (
            [AsParameters] GetHrAnalyticsRequest request,
            ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetHRAnalyticsEndpoint))
        .WithSummary("Get HR analytics")
        .WithDescription("Retrieves comprehensive HR metrics and KPIs including headcount, attendance, payroll, performance, and more")
        .Produces<HrAnalyticsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Analytics))
        .MapToApiVersion(1);
    }
}

