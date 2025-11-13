using FSH.Starter.WebApi.HumanResources.Application.Designations.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1.Designations;

/// <summary>
/// Endpoint for updating a designation.
/// </summary>
public static class UpdateDesignationEndpoint
{
    internal static RouteHandlerBuilder MapDesignationUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateDesignationCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateDesignationEndpoint))
            .WithSummary("Updates a designation")
            .WithDescription("Updates designation information")
            .Produces<UpdateDesignationResponse>()
            .RequirePermission("Permissions.Designations.Update")
            .MapToApiVersion(1);
    }
}

