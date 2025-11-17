namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.HRAnalytics.v1;

using FSH.Starter.WebApi.HumanResources.Application.HRAnalytics.Get.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for retrieving department-specific HR analytics.
/// </summary>
public static class GetDepartmentAnalyticsEndpoint
{
    /// <summary>
    /// Maps the get department analytics endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGetDepartmentAnalyticsEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/department/{departmentId}", async (
            DefaultIdType departmentId,
            [AsParameters] GetHrAnalyticsRequest baseRequest,
            ISender mediator) =>
        {
            var request = new GetHrAnalyticsRequest(
                baseRequest.FromDate,
                baseRequest.ToDate,
                departmentId);

            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetDepartmentAnalyticsEndpoint))
        .WithSummary("Get department analytics")
        .WithDescription("Retrieves HR metrics and KPIs filtered by specific department")
        .Produces<HrAnalyticsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Analytics))
        .MapToApiVersion(1);
    }
}

