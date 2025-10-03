namespace Store.Infrastructure.Endpoints.WholesalePricings.v1;

public static class DeactivateWholesalePricingEndpoint
{
    internal static RouteHandlerBuilder MapDeactivateWholesalePricingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateWholesalePricingCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("DeactivateWholesalePricing")
        .WithSummary("Deactivate wholesale pricing")
        .WithDescription("Marks a wholesale pricing entry as inactive")
        .Produces<DeactivateWholesalePricingResponse>()
        .MapToApiVersion(1);
    }
}
