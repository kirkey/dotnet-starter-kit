using FSH.Starter.WebApi.Store.Application.WholesalePricings.Get.v1;

namespace Store.Infrastructure.Endpoints.WholesalePricings.v1;

public static class GetWholesalePricingEndpoint
{
    internal static RouteHandlerBuilder MapGetWholesalePricingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetWholesalePricingQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetWholesalePricing")
        .WithSummary("Get wholesale pricing by ID")
        .WithDescription("Retrieves a wholesale pricing entry by its unique identifier")
        .MapToApiVersion(1);
    }
}

