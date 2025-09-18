using FSH.Starter.WebApi.Store.Application.Categories.Get.v1;
using FSH.Starter.WebApi.Store.Application.Categories.Search.v1;

namespace Store.Infrastructure.Endpoints.Categories.v1;

public static class SearchCategoriesEndpoint
{
    internal static RouteHandlerBuilder MapSearchCategoriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (HttpRequest req, ISender sender) =>
        {
            // Bind query params to SearchCategoriesCommand
            var cmd = new SearchCategoriesCommand
            {
                PageNumber = int.TryParse(req.Query["pageNumber"], out var pn) ? pn : 1,
                PageSize = int.TryParse(req.Query["pageSize"], out var ps) ? ps : 10,
                Name = req.Query["name"],
                Code = req.Query["code"],
                IsActive = string.IsNullOrEmpty(req.Query["isActive"]) ? null : (bool?)bool.Parse(req.Query["isActive"])
            };

            var result = await sender.Send(cmd).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchCategories")
        .WithSummary("Search categories")
        .WithDescription("Searches categories with pagination and filters")
        .Produces<PagedList<CategoryResponse>>()
        .MapToApiVersion(1);
    }
}

