using FSH.Starter.WebApi.Store.Application.WholesaleContracts.Get.v1;

namespace Store.Infrastructure.Endpoints.WholesaleContracts.v1;

/// <summary>
/// Endpoint for retrieving a wholesale contract by ID.
/// </summary>
public static class GetWholesaleContractEndpoint
{
    /// <summary>
    /// Maps the get wholesale contract endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for get wholesale contract endpoint</returns>
    internal static RouteHandlerBuilder MapGetWholesaleContractEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetWholesaleContractQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetWholesaleContract")
        .WithSummary("Get a wholesale contract")
        .WithDescription("Retrieves a wholesale contract by ID")
        .Produces<GetWholesaleContractResponse>()
        .MapToApiVersion(1);
    }
}
