namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDashboards;

using v1;

/// <summary>
/// Employee Dashboards endpoints coordinator.
/// </summary>
public static class EmployeeDashboardsEndpoints
{
    /// <summary>
    /// Maps all employee dashboard endpoints.
    /// </summary>
    public static void MapEmployeeDashboardsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("employee-dashboards").WithTags("Employee Dashboards");
        group.MapGetEmployeeDashboardEndpoint();
        group.MapGetTeamDashboardEndpoint();
    }
}

