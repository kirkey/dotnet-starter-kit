using FSH.Starter.WebApi.Store.Application.WholesaleContracts.Create.v1;

namespace Store.Infrastructure.Endpoints.WholesaleContracts.v1;

/// <summary>
/// Endpoint for creating a new wholesale contract.
/// </summary>
public static class CreateWholesaleContractEndpoint
{
    /// <summary>
    /// Maps the create wholesale contract endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for create wholesale contract endpoint</returns>
    internal static RouteHandlerBuilder MapCreateWholesaleContractEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateWholesaleContractCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/wholesale-contracts/{result.Id}", result);
        })
        .WithName("CreateWholesaleContract")
        .WithSummary("Create a new wholesale contract")
        .WithDescription("Creates a new wholesale contract")
        .Produces<CreateWholesaleContractResponse>()
        .MapToApiVersion(1);
    }
}
