namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Taxes.v1;

using FSH.Starter.WebApi.HumanResources.Application.Taxes.Search.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for searching and filtering tax master configurations.
/// </summary>
public static class SearchTaxesEndpoint
{
    /// <summary>
    /// Maps the search taxes endpoint.
    /// </summary>
    /// <param name="group">Route group builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    public static RouteHandlerBuilder MapSearchTaxesEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/search", async (SearchTaxesRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(SearchTaxesEndpoint))
        .WithSummary("Search tax master configurations")
        .WithDescription("Searches and filters tax master configurations with pagination support. " +
                         "Supports filtering by code, tax type, jurisdiction, and active status.")
        .Produces<PagedList<TaxDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Taxes))
        .MapToApiVersion(1);
    }
}

