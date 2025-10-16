using Accounting.Application.Banks.Update.v1;

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
    internal static RouteHandlerBuilder MapBankUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, BankUpdateCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("The ID in the URL does not match the ID in the request body.");
                }

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BankUpdateEndpoint))
            .WithSummary("Update an existing bank")
            .WithDescription("Updates an existing bank in the accounting system with comprehensive validation.")
            .Produces<BankUpdateResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

