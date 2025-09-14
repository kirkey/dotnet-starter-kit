using FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

public static class SearchCycleCountsEndpoint
{
    internal static RouteHandlerBuilder MapSearchCycleCountsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (HttpRequest req, ISender sender) =>
        {
            var cmd = new SearchCycleCountsCommand
            {
                PageNumber = int.TryParse(req.Query["pageNumber"], out var pn) ? pn : 1,
                PageSize = int.TryParse(req.Query["pageSize"], out var ps) ? ps : 10,
                CountNumber = req.Query["countNumber"],
                Status = req.Query["status"],
                WarehouseId = string.IsNullOrEmpty(req.Query["warehouseId"]) ? null : (DefaultIdType?)DefaultIdType.Parse(req.Query["warehouseId"].ToString())
            };

            var result = await sender.Send(cmd).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchCycleCounts")
        .WithSummary("Search cycle counts")
        .WithDescription("Searches cycle counts with pagination and filters")
        .MapToApiVersion(1);
    }
}
