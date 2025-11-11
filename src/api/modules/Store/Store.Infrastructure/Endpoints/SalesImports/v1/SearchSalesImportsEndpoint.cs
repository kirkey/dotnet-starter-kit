using FSH.Starter.WebApi.Store.Application.SalesImports.Search.v1;

namespace Store.Infrastructure.Endpoints.SalesImports.v1;

/// <summary>
/// Endpoint for searching sales imports with filtering.
/// </summary>
public static class SearchSalesImportsEndpoint
{
    internal static RouteHandlerBuilder MapSearchSalesImportsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchSalesImportsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchSalesImportsEndpoint))
            .WithSummary("Search sales imports")
            .WithDescription("Searches sales imports with filtering and pagination")
            .Produces<PagedList<SalesImportResponse>>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}

