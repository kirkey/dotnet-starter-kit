using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits.v1;

/// <summary>
/// Endpoint for updating an organizational unit.
/// </summary>
public static class UpdateOrganizationalUnitEndpoint
{
    internal static RouteHandlerBuilder MapOrganizationalUnitUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateOrganizationalUnitCommand request, ISender mediator) =>
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
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Organization))
            .MapToApiVersion(1);
    }
}
