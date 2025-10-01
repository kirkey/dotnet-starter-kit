using FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;
using FSH.Starter.WebApi.Store.Application.Suppliers.Search.v1;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class SearchSuppliersEndpoint
{
    internal static RouteHandlerBuilder MapSearchSuppliersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchSuppliersCommand command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchSuppliersEndpoint))
            .WithSummary("Search Suppliers")
            .WithDescription("Searches Suppliers with pagination and filters")
            .Produces<PagedList<SupplierResponse>>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
