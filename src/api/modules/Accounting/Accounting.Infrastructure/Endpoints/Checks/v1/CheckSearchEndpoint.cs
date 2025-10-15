using Accounting.Application.Checks.Search.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for searching checks with filtering and pagination.
/// </summary>
public static class CheckSearchEndpoint
{
    internal static RouteHandlerBuilder MapCheckSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (CheckSearchQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckSearchEndpoint))
            .WithSummary("Search checks")
            .WithDescription("Search and filter checks with pagination, status filtering, and date ranges")
            .Produces<PagedList<CheckSearchResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

