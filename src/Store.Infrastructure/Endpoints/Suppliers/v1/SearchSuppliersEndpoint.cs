using FSH.Starter.WebApi.Store.Application.Suppliers.Search.v1;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class SearchSuppliersEndpoint
{
    internal static RouteHandlerBuilder MapSearchSuppliersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (HttpRequest req, ISender sender) =>
        {
            var cmd = new SearchSuppliersCommand
            {
                PageNumber = int.TryParse(req.Query["pageNumber"], out var pn) ? pn : 1,
                PageSize = int.TryParse(req.Query["pageSize"], out var ps) ? ps : 10,
                Name = req.Query["name"],
                Code = req.Query["code"],
                City = req.Query["city"],
                Country = req.Query["country"]
            };

            var result = await sender.Send(cmd).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchSuppliers")
        .WithSummary("Search suppliers")
        .WithDescription("Searches suppliers with pagination and filters")
        .MapToApiVersion(1);
    }
}

