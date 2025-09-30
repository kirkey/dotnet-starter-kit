using Accounting.Application.Payees.Create.v1;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

/// <summary>
/// Endpoint for creating new payees in the accounting system.
/// Follows REST API conventions with proper documentation and error handling.
/// </summary>
public static class PayeeCreateEndpoint
{
    /// <summary>
    /// Maps the payee creation endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapPayeeCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (PayeeCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PayeeCreateEndpoint))
            .WithSummary("Create a new payee")
            .WithDescription("Creates a new payee in the accounting system with comprehensive validation and returns the created payee ID.")
            .Produces<PayeeCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
