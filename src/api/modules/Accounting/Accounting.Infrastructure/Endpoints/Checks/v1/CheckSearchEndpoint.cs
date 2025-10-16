using Accounting.Application.Checks.Search.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for searching checks with filtering and pagination.
/// Supports advanced filtering by check number, account, payee, amount range, and date range.
/// </summary>
public static class CheckSearchEndpoint
{
    /// <summary>
    /// Maps the check search endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
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
            .WithDescription("Search and filter checks with pagination, status filtering, date ranges, and amount ranges. Supports advanced filtering by check number, bank account, payee name, and print/stop payment status.")
            .Produces<PagedList<CheckSearchResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

