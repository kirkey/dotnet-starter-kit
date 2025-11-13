using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1.Designations;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1;

/// <summary>
/// Extension methods for designation endpoints.
/// </summary>
public static class DesignationEndpointExtensions
{
    /// <summary>
    /// Maps all designation endpoints.
    /// </summary>
    public static IEndpointRouteBuilder MapDesignationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("designations").WithTags("designations");

        group.MapDesignationCreateEndpoint();
        group.MapDesignationGetEndpoint();
        group.MapDesignationsSearchEndpoint();
        group.MapDesignationUpdateEndpoint();
        group.MapDesignationDeleteEndpoint();

        return endpoints;
    }
}

