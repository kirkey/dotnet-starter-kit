using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Designations.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Designations;

/// <summary>
/// Endpoint configuration for Designations module.
/// </summary>
public static class DesignationsEndpoints
{
    /// <summary>
    /// Maps all Designations endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapDesignationsEndpoints(this IEndpointRouteBuilder app)
    {
        var designationsGroup = app.MapGroup("/designations")
            .WithTags("Designations")
            .WithDescription("Endpoints for managing job designations and positions");

        // Version 1 endpoints
        designationsGroup.MapDesignationCreateEndpoint();
        designationsGroup.MapDesignationUpdateEndpoint();
        designationsGroup.MapDesignationDeleteEndpoint();
        designationsGroup.MapDesignationGetEndpoint();
        designationsGroup.MapDesignationsSearchEndpoint();

        return app;
    }
}

