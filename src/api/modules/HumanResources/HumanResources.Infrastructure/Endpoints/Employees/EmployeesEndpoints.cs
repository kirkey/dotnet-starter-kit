using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees;

/// <summary>
/// Endpoint configuration for Employees module.
/// </summary>
public static class EmployeesEndpoints
{
    /// <summary>
    /// Maps all Employees endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapEmployeesEndpoints(this IEndpointRouteBuilder app)
    {
        var employeesGroup = app.MapGroup("/employees")
            .WithTags("Employees")
            .WithDescription("Endpoints for managing employee information and lifecycle");

        // Version 1 endpoints
        employeesGroup.MapCreateEmployeeEndpoint();
        employeesGroup.MapUpdateEmployeeEndpoint();
        employeesGroup.MapDeleteEmployeeEndpoint();
        employeesGroup.MapGetEmployeeEndpoint();
        employeesGroup.MapSearchEmployeesEndpoint();

        return app;
    }
}

