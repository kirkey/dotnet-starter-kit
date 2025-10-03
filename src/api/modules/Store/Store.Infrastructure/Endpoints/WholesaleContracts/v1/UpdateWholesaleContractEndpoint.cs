namespace Store.Infrastructure.Endpoints.WholesaleContracts.v1;

/// <summary>
/// Endpoint for updating a wholesale contract.
/// </summary>
public static class UpdateWholesaleContractEndpoint
{
    /// <summary>
    /// Maps the update wholesale contract endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for update wholesale contract endpoint</returns>
    internal static RouteHandlerBuilder MapUpdateWholesaleContractEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWholesaleContractCommand command, ISender sender) =>
        {
            var updateCommand = command with { Id = id };
            var result = await sender.Send(updateCommand).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateWholesaleContract")
        .WithSummary("Update a wholesale contract")
        .WithDescription("Updates an existing wholesale contract")
        .Produces<UpdateWholesaleContractResponse>()
        .MapToApiVersion(1);
    }
}
