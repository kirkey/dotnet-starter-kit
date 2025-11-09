using Accounting.Application.GeneralLedgers.Search.v1;

namespace Accounting.Infrastructure.Endpoints.GeneralLedger.v1;

/// <summary>
/// Endpoint for searching general ledger entries.
/// </summary>
public static class GeneralLedgerSearchEndpoint
{
    /// <summary>
    /// Maps the general ledger search endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapGeneralLedgerSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (GeneralLedgerSearchRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GeneralLedgerSearchEndpoint))
            .WithSummary("Search general ledger entries")
            .WithDescription("Searches general ledger entries with filtering and pagination")
            .Produces<PagedList<GeneralLedgerSearchResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
