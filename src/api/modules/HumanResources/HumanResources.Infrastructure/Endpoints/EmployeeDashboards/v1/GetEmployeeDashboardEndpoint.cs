using FSH.Framework.Core.Identity.Users.Abstractions;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDashboards.v1;

using FSH.Starter.WebApi.HumanResources.Application.EmployeeDashboards.Get.v1;

/// <summary>
/// Endpoint for retrieving personal employee dashboard.
/// </summary>
public static class GetEmployeeDashboardEndpoint
{
    /// <summary>
    /// Maps the get employee dashboard endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGetEmployeeDashboardEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/me", async (ICurrentUser currentUser, ISender mediator) =>
        {
            var request = new GetEmployeeDashboardRequest(currentUser.GetUserId());
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetEmployeeDashboardEndpoint))
        .WithSummary("Get personal employee dashboard")
        .WithDescription("Retrieves aggregated dashboard data for the current employee including leave, attendance, payroll, and pending approvals")
        .Produces<EmployeeDashboardResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .MapToApiVersion(1);
    }
}

