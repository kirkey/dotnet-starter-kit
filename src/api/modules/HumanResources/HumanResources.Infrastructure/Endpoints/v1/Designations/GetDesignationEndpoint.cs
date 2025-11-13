using FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1.Designations;

/// <summary>
/// Endpoint for getting a designation by ID.
/// </summary>
public static class GetDesignationEndpoint
{
    internal static RouteHandlerBuilder MapDesignationGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetDesignationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetDesignationEndpoint))
            .WithSummary("Gets designation by ID")
            .WithDescription("Retrieves designation details by ID")
            .Produces<DesignationResponse>()
            .RequirePermission("Permissions.Designations.View")
            .MapToApiVersion(1);
    }
}

