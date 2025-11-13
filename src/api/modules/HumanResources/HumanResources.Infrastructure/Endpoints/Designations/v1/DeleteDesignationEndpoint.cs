using FSH.Starter.WebApi.HumanResources.Application.Designations.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Designations.v1;

/// <summary>
/// Endpoint for deleting a designation.
/// </summary>
public static class DeleteDesignationEndpoint
{
    internal static RouteHandlerBuilder MapDesignationDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteDesignationCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteDesignationEndpoint))
            .WithSummary("Deletes a designation")
            .WithDescription("Deletes a designation")
            .Produces<DeleteDesignationResponse>()
            .RequirePermission("Permissions.Designations.Delete")
            .MapToApiVersion(1);
    }
}

