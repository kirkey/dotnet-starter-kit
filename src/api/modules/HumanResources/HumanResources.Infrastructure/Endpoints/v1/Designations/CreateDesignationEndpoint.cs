using FSH.Starter.WebApi.HumanResources.Application.Designations.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1.Designations;

/// <summary>
/// Endpoint for creating a designation.
/// </summary>
public static class CreateDesignationEndpoint
{
    internal static RouteHandlerBuilder MapDesignationCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateDesignationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateDesignationEndpoint))
            .WithSummary("Creates a new designation")
            .WithDescription("Creates a new designation in an organizational unit")
            .Produces<CreateDesignationResponse>()
            .RequirePermission("Permissions.Designations.Create")
            .MapToApiVersion(1);
    }
}

