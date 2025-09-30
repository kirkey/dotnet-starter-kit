using Accounting.Application.Payees.Get.v1;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

/// <summary>
/// Endpoint for retrieving a specific payee by ID from the accounting system.
/// Follows REST API conventions with proper documentation and error handling.
/// </summary>
public static class PayeeGetEndpoint
{
    /// <summary>
    /// Maps the payee retrieval endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapPayeeGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new PayeeGetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PayeeGetEndpoint))
            .WithSummary("Get a payee by ID")
            .WithDescription("Retrieves a specific payee from the accounting system using its unique identifier.")
            .Produces<PayeeResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
