using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDashboards.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDashboards;

/// <summary>
/// Endpoint configuration for EmployeeDashboards module.
/// </summary>
public class EmployeeDashboardsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all EmployeeDashboards endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/employee-dashboards").WithTags("employee-dashboards");

        group.MapGet("/me", async (ICurrentUser currentUser, ISender mediator) =>
            {
                var request = new GetEmployeeDashboardRequest(currentUser.GetUserId());
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetEmployeeDashboard")
            .WithSummary("Get personal employee dashboard")
            .WithDescription("Retrieves aggregated dashboard data for the current employee including leave, attendance, payroll, and pending approvals")
            .Produces<EmployeeDashboardResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        group.MapGet("/team/{employeeId:guid}", async (DefaultIdType employeeId, ISender mediator) =>
            {
                var request = new GetEmployeeDashboardRequest(employeeId);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetTeamDashboard")
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

