using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1.OrganizationalUnits;

/// <summary>
/// Endpoint for updating an organizational unit.
/// </summary>
public static class UpdateOrganizationalUnitEndpoint
{
    internal static RouteHandlerBuilder MapOrganizationalUnitUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateOrganizationalUnitCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateOrganizationalUnitEndpoint))
            .WithSummary("Updates an organizational unit")
            .WithDescription("Updates organizational unit information")
            .Produces<UpdateOrganizationalUnitResponse>()
            .RequirePermission("Permissions.OrganizationalUnits.Update")
            .MapToApiVersion(1);
    }
}

