using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits.v1;

/// <summary>
/// Endpoint for searching organizational units.
/// </summary>
public static class SearchOrganizationalUnitsEndpoint
{
    internal static RouteHandlerBuilder MapOrganizationalUnitsSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchOrganizationalUnitsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchOrganizationalUnitsEndpoint))
            .WithSummary("Searches organizational units")
            .WithDescription("Searches organizational units with pagination and filters")
            .Produces<PagedList<OrganizationalUnitResponse>>()
            .RequirePermission("Permissions.OrganizationalUnits.View")
            .MapToApiVersion(1);
    }
}

