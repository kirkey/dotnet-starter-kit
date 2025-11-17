namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDashboards.v1;

using FSH.Starter.WebApi.HumanResources.Application.EmployeeDashboards.Get.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for retrieving team dashboard (for managers).
/// </summary>
public static class GetTeamDashboardEndpoint
{
    /// <summary>
    /// Maps the get team dashboard endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGetTeamDashboardEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/team/{employeeId}", async (DefaultIdType employeeId, ISender mediator) =>
        {
            var request = new GetEmployeeDashboardRequest(employeeId);
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetTeamDashboardEndpoint))
        .WithSummary("Get team member dashboard")
        .WithDescription("Retrieves dashboard data for a team member (managers only)")
        .Produces<EmployeeDashboardResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Dashboard))
        .MapToApiVersion(1);
    }
}

