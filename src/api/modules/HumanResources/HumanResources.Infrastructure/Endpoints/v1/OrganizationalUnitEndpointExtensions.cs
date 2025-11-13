using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1.OrganizationalUnits;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1;

/// <summary>
/// Extension methods for organizational unit endpoints.
/// </summary>
public static class OrganizationalUnitEndpointExtensions
{
    /// <summary>
    /// Maps all organizational unit endpoints.
    /// </summary>
    public static IEndpointRouteBuilder MapOrganizationalUnitEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("organizational-units").WithTags("organizational-units");

        group.MapOrganizationalUnitCreateEndpoint();
        group.MapOrganizationalUnitGetEndpoint();
        group.MapOrganizationalUnitsSearchEndpoint();
        group.MapOrganizationalUnitUpdateEndpoint();
        group.MapOrganizationalUnitDeleteEndpoint();

        return endpoints;
    }
}

