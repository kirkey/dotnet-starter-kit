using Accounting.Application.DeferredRevenue.Queries;

// Endpoint for searching deferred revenues
namespace Accounting.Infrastructure.Endpoints.DeferredRevenue.v1;

public static class DeferredRevenueSearchEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchDeferredRevenuesQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeferredRevenueSearchEndpoint))
            .WithSummary("Search deferred revenues")
            .WithDescription("Searches deferred revenue entries with filters and pagination")
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
