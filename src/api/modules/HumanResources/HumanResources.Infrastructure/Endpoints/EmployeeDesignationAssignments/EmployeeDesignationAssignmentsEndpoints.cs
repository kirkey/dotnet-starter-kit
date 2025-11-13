using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDesignationAssignments.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDesignationAssignments;

/// <summary>
/// Endpoint configuration for EmployeeDesignationAssignments module.
/// </summary>
public static class EmployeeDesignationAssignmentsEndpoints
{
    /// <summary>
    /// Maps all EmployeeDesignationAssignments endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapEmployeeDesignationAssignmentsEndpoints(this IEndpointRouteBuilder app)
    {
        var assignmentsGroup = app.MapGroup("/employee-designations")
            .WithTags("Employee Designations")
            .WithDescription("Endpoints for managing employee designation assignments (plantilla and acting as)");

        // Version 1 endpoints
        assignmentsGroup.MapAssignPlantillaDesignationEndpoint();
        assignmentsGroup.MapAssignActingAsDesignationEndpoint();

        return app;
    }
}

