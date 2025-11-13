using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents;

/// <summary>
/// Endpoint configuration for EmployeeDependents module.
/// </summary>
public static class EmployeeDependentsEndpoints
{
    /// <summary>
    /// Maps all EmployeeDependents endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapEmployeeDependentsEndpoints(this IEndpointRouteBuilder app)
    {
        var dependentsGroup = app.MapGroup("/employee-dependents")
            .WithTags("Employee Dependents")
            .WithDescription("Endpoints for managing employee dependents (family members, beneficiaries)");

        // Version 1 endpoints
        dependentsGroup.MapCreateEmployeeDependentEndpoint();
        dependentsGroup.MapGetEmployeeDependentEndpoint();
        dependentsGroup.MapSearchEmployeeDependentsEndpoint();
        dependentsGroup.MapUpdateEmployeeDependentEndpoint();
        dependentsGroup.MapDeleteEmployeeDependentEndpoint();

        return app;
    }
}

