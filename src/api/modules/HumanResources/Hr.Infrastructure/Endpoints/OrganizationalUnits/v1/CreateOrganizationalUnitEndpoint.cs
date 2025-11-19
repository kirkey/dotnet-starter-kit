using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.OrganizationalUnits.v1;

/// <summary>
/// Endpoint for creating an organizational unit.
/// </summary>
public static class CreateOrganizationalUnitEndpoint
{
    internal static RouteHandlerBuilder MapOrganizationalUnitCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateOrganizationalUnitCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetOrganizationalUnitEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateOrganizationalUnitEndpoint))
            .WithSummary("Creates a new organizational unit")
            .WithDescription("Creates a new organizational unit (Department, Division, or Section)")
            .Produces<CreateOrganizationalUnitResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Organization))
            .MapToApiVersion(1);
    }
}
