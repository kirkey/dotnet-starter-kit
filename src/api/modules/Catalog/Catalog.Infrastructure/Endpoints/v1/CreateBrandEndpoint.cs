using Shared.Authorization;

namespace FSH.Starter.WebApi.Catalog.Infrastructure.Endpoints.v1;

/// <summary>
/// Endpoint for creating a new brand.
/// Requires Create permission on Brands resource.
/// </summary>
public static class CreateBrandEndpoint
{
    /// <summary>
    /// Maps the create brand endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>A route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapBrandCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBrandCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateBrandEndpoint))
            .WithSummary("Creates a new brand")
            .WithDescription("Creates a new brand with the specified name, description, and notes.")
            .Produces<CreateBrandResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Brands))
            .MapToApiVersion(1);
    }
}
