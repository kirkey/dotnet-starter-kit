using Accounting.Application.Payees.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

/// <summary>
/// Endpoint for deleting payees from the accounting system.
/// Follows REST API conventions with proper documentation and error handling.
/// </summary>
public static class PayeeDeleteEndpoint
{
    /// <summary>
    /// Maps the payee deletion endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapPayeeDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new PayeeDeleteCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(PayeeDeleteEndpoint))
            .WithSummary("Delete a payee")
            .WithDescription("Deletes a payee from the accounting system. Returns 204 No Content on successful deletion.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
