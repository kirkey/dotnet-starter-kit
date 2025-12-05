using Accounting.Application.Banks.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Banks.v1;

/// <summary>
/// Endpoint for deleting banks from the accounting system.
/// </summary>
public static class BankDeleteEndpoint
{
    /// <summary>
    /// Maps the bank deletion endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapDeleteBankEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteBankCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BankDeleteEndpoint))
            .WithSummary("Delete a bank")
            .WithDescription("Deletes a bank from the accounting system. Consider deactivating instead of deleting to preserve historical data.")
            .Produces<BankDeleteResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

