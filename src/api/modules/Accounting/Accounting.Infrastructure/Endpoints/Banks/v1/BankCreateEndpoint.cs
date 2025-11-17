using Accounting.Application.Banks.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Banks.v1;

/// <summary>
/// Endpoint for creating new banks in the accounting system.
/// Follows REST API conventions with proper documentation and error handling.
/// </summary>
public static class BankCreateEndpoint
{
    /// <summary>
    /// Maps the bank creation endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapBankCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (BankCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BankCreateEndpoint))
            .WithSummary("Create a new bank")
            .WithDescription("Creates a new bank in the accounting system with comprehensive validation and returns the created bank ID.")
            .Produces<BankCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
