using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits;

/// <summary>
/// Endpoint configuration for OrganizationalUnits module.
/// </summary>
public static class OrganizationalUnitsEndpoints
{
    /// <summary>
    /// Maps all OrganizationalUnits endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapOrganizationalUnitsEndpoints(this IEndpointRouteBuilder app)
    {
        var orgUnitsGroup = app.MapGroup("/organizational-units")
            .WithTags("Organizational Units")
            .WithDescription("Endpoints for managing organizational units (departments, divisions, sections)");

        // Version 1 endpoints
        orgUnitsGroup.MapOrganizationalUnitCreateEndpoint();
        orgUnitsGroup.MapOrganizationalUnitUpdateEndpoint();
        orgUnitsGroup.MapOrganizationalUnitDeleteEndpoint();
        orgUnitsGroup.MapOrganizationalUnitGetEndpoint();
        orgUnitsGroup.MapOrganizationalUnitsSearchEndpoint();

        return app;
    }
}

