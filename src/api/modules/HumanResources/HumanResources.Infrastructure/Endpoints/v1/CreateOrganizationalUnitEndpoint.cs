using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1;

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
                return Results.Ok(response);
            })
            .WithName(nameof(CreateOrganizationalUnitEndpoint))
            .WithSummary("Creates a new organizational unit")
            .WithDescription("Creates a new organizational unit (Department, Division, or Section)")
            .Produces<CreateOrganizationalUnitResponse>()
            .RequirePermission("Permissions.OrganizationalUnits.Create")
            .MapToApiVersion(1);
    }
}

