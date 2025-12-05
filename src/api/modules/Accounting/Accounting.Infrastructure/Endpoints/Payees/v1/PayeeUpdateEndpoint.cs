using Accounting.Application.Payees.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

/// <summary>
/// Endpoint for updating existing payees in the accounting system.
/// Follows REST API conventions with proper documentation and error handling.
/// </summary>
public static class PayeeUpdateEndpoint
{
    /// <summary>
    /// Maps the payee update endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapPayeeUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdatePayeeCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID in URL must match ID in request body.");
                
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PayeeUpdateEndpoint))
            .WithSummary("Update an existing payee")
            .WithDescription("Updates an existing payee in the accounting system with comprehensive validation.")
            .Produces<PayeeUpdateResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
