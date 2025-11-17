using Accounting.Application.AccountReconciliations.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliations.v1;

/// <summary>
/// Endpoint for searching account reconciliations.
/// </summary>
public static class SearchAccountReconciliationsEndpoint
{
    internal static RouteHandlerBuilder MapSearchAccountReconciliationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchAccountReconciliationsRequest request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchAccountReconciliationsEndpoint))
            .WithSummary("Search Account Reconciliations")
            .WithDescription("Search and filter account reconciliations with pagination and filtering support")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

