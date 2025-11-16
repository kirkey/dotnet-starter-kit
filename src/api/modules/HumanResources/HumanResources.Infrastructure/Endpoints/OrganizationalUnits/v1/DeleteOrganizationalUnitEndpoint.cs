using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Delete.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits.v1;

/// <summary>
/// Endpoint for deleting an organizational unit.
/// </summary>
public static class DeleteOrganizationalUnitEndpoint
{
    internal static RouteHandlerBuilder MapOrganizationalUnitDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteOrganizationalUnitCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteOrganizationalUnitEndpoint))
            .WithSummary("Deletes an organizational unit")
            .WithDescription("Deletes an organizational unit if it has no children")
            .Produces<DeleteOrganizationalUnitResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Organization))
            .MapToApiVersion(1);
    }
}
