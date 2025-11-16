using FSH.Starter.WebApi.HumanResources.Application.Designations.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Designations.v1;

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
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Organization))
            .MapToApiVersion(1);
    }
}

