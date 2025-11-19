using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments;

/// <summary>
/// Endpoint configuration for DesignationAssignments module.
/// </summary>
public static class DesignationAssignmentsEndpoints
{
    /// <summary>
    /// Maps all DesignationAssignments endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapDesignationAssignmentsEndpoints(this IEndpointRouteBuilder app)
    {
        var assignmentsGroup = app.MapGroup("/employee-designations")
            .WithTags("Employee Designations")
            .WithDescription("Endpoints for managing employee designation assignments (plantilla and acting as)");

        // Version 1 endpoints
        assignmentsGroup.MapAssignPlantillaDesignationEndpoint();
        assignmentsGroup.MapAssignActingAsDesignationEndpoint();
        assignmentsGroup.MapGetDesignationAssignmentEndpoint();
        assignmentsGroup.MapEndDesignationAssignmentEndpoint();
        assignmentsGroup.MapSearchEmployeeHistoryEndpoint();

        return app;
    }
}

