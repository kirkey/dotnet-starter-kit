namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Taxes.v1;

using FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for updating an existing tax master configuration.
/// </summary>
public static class UpdateTaxEndpoint
{
    /// <summary>
    /// Maps the update tax endpoint.
    /// </summary>
    /// <param name="group">Route group builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    public static RouteHandlerBuilder MapUpdateTaxEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPut("/{id}", async (DefaultIdType id, UpdateTaxCommand request, ISender mediator) =>
        {
            if (id != request.Id)
                return Results.BadRequest("Route ID does not match request ID");

            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(UpdateTaxEndpoint))
        .WithSummary("Update tax master configuration")
        .WithDescription("Updates an existing tax master configuration. Only provided fields are updated.")
        .Produces<DefaultIdType>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Taxes))
        .MapToApiVersion(1);
    }
}

