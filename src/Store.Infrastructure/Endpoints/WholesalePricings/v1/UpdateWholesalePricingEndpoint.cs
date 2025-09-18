using FSH.Starter.WebApi.Store.Application.WholesalePricings.UpdatePricing.v1;

namespace Store.Infrastructure.Endpoints.WholesalePricings.v1;

public static class UpdateWholesalePricingEndpoint
{
    internal static RouteHandlerBuilder MapUpdateWholesalePricingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWholesalePricingCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateWholesalePricing")
        .WithSummary("Update wholesale pricing tier")
        .WithDescription("Updates tier price and discount percentage for an existing wholesale pricing entry")
        .Produces<UpdateWholesalePricingResponse>()
        .MapToApiVersion(1);
    }
}
