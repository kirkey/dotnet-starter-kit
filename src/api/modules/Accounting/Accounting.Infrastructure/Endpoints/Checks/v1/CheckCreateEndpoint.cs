using Accounting.Application.Checks.Create.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for creating/registering a new check in the accounting system.
/// Initializes a check in "Available" status for later use in payments.
/// </summary>
public static class CheckCreateEndpoint
{
    /// <summary>
    /// Maps the check creation endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapCheckCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CheckCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/checks/{response.Id}", response);
            })
            .WithName(nameof(CheckCreateEndpoint))
            .WithSummary("Register a new check")
            .WithDescription("Register a new check in the system for later use in payments. Creates a check in 'Available' status.")
            .Produces<CheckCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
