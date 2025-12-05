using Accounting.Application.Banks.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Banks.v1;

/// <summary>
/// Endpoint for updating existing banks in the accounting system.
/// </summary>
public static class BankUpdateEndpoint
{
    /// <summary>
    /// Maps the bank update endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapUpdateBankEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, BankUpdateCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BankUpdateEndpoint))
            .WithSummary("Update an existing bank")
            .WithDescription("Updates an existing bank in the accounting system with comprehensive validation.")
            .Produces<BankUpdateResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

