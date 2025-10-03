using ItemSupplierResponse = FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1.ItemSupplierResponse;
using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Search.v1;

namespace Store.Infrastructure.Endpoints.ItemSuppliers.v1;

public static class SearchItemSuppliersEndpoint
{
    internal static RouteHandlerBuilder MapSearchItemSuppliersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchItemSuppliersCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchItemSuppliersEndpoint))
            .WithSummary("Search item-supplier relationships")
            .WithDescription("Search and filter item-supplier relationships with pagination support")
            .RequirePermission("Permissions.Store.View")
            .Produces<PagedList<ItemSupplierResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .MapToApiVersion(1);
    }
}
