using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1;

/// <summary>
/// Endpoint for getting an organizational unit by ID.
/// </summary>
public static class GetOrganizationalUnitEndpoint
{
    internal static RouteHandlerBuilder MapOrganizationalUnitGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetOrganizationalUnitRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetOrganizationalUnitEndpoint))
            .WithSummary("Gets organizational unit by ID")
            .WithDescription("Retrieves organizational unit details by ID")
            .Produces<OrganizationalUnitResponse>()
            .RequirePermission("Permissions.OrganizationalUnits.View")
            .MapToApiVersion(1);
    }
}

