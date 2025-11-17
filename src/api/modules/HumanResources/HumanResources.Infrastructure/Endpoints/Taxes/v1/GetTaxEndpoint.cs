namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Taxes.v1;

using FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for retrieving a tax master configuration by ID.
/// </summary>
public static class GetTaxEndpoint
{
    /// <summary>
    /// Maps the get tax endpoint.
    /// </summary>
    /// <param name="group">Route group builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    public static RouteHandlerBuilder MapGetTaxEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetTaxRequest(id);
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetTaxEndpoint))
        .WithSummary("Get tax master configuration")
        .WithDescription("Retrieves a tax master configuration by ID with all details.")
        .Produces<TaxResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Taxes))
        .MapToApiVersion(1);
    }
}

