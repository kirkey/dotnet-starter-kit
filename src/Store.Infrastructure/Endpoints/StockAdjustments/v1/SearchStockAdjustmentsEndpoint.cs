using FSH.Starter.WebApi.Store.Application.StockAdjustments.Search.v1;
using FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

namespace Store.Infrastructure.Endpoints.StockAdjustments.v1;

public static class SearchStockAdjustmentsEndpoint
{
    internal static RouteHandlerBuilder MapSearchStockAdjustmentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async ([AsParameters] SearchStockAdjustmentsCommand query, ISender sender) =>
        {
            var result = await sender.Send(query).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchStockAdjustments")
        .WithSummary("Search stock adjustments")
        .WithDescription("Retrieves a paginated list of stock adjustments with optional filtering")
        .Produces<PagedList<StockAdjustmentResponse>>()
        .MapToApiVersion(1);
    }
}
