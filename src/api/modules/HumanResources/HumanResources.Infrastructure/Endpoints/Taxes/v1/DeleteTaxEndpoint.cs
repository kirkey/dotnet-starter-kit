namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Taxes.v1;

using FSH.Starter.WebApi.HumanResources.Application.Taxes.Delete.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for deleting a tax master configuration.
/// </summary>
public static class DeleteTaxEndpoint
{
    /// <summary>
    /// Maps the delete tax endpoint.
    /// </summary>
    /// <param name="group">Route group builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    public static RouteHandlerBuilder MapDeleteTaxEndpoint(this RouteGroupBuilder group)
    {
        return group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new DeleteTaxCommand(id);
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(DeleteTaxEndpoint))
        .WithSummary("Delete tax master configuration")
        .WithDescription("Deletes a tax master configuration by ID.")
        .Produces<DefaultIdType>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Taxes))
        .MapToApiVersion(1);
    }
}

