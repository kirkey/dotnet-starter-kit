namespace Store.Infrastructure.Endpoints.WholesalePricings.v1;

public static class CreateWholesalePricingEndpoint
{
    internal static RouteHandlerBuilder MapCreateWholesalePricingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateWholesalePricingCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/wholesale-pricings/{result.Id}", result);
        })
        .WithName("CreateWholesalePricing")
        .WithSummary("Create a new wholesale pricing")
        .WithDescription("Creates a new wholesale pricing tier for a contract and grocery item")
        .Produces<CreateWholesalePricingResponse>()
        .MapToApiVersion(1);
    }
}
