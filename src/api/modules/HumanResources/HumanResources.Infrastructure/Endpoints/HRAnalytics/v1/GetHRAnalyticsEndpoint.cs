namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.HRAnalytics.v1;

using FSH.Starter.WebApi.HumanResources.Application.HRAnalytics.Get.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for retrieving company-wide HR analytics.
/// </summary>
public static class GetHrAnalyticsEndpoint
{
    /// <summary>
    /// Maps the get HR analytics endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGetHrAnalyticsEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/", async (
            DateTime? fromDate,
            DateTime? toDate,
            DefaultIdType? departmentId,
            ISender mediator) =>
        {
            var request = new GetHrAnalyticsRequest(fromDate, toDate, departmentId);
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetHrAnalyticsEndpoint))
        .WithSummary("Get HR analytics")
        .WithDescription("Retrieves comprehensive HR metrics and KPIs including headcount, attendance, payroll, performance, and more")
        .Produces<HrAnalyticsResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Analytics))
        .MapToApiVersion(1);
    }
}

