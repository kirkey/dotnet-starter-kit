using Accounting.Application.Checks.Update.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for updating an existing check in the accounting system.
/// Updates check details such as bank account code, bank, and notes.
/// BankAccountName and BankName are automatically populated from their respective entities.
/// </summary>
public static class CheckUpdateEndpoint
{
    /// <summary>
    /// Maps the check update endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapCheckUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, CheckUpdateCommand command, ISender mediator) =>
            {
                if (id != command.CheckId)
                {
                    return Results.BadRequest("The ID in the URL does not match the ID in the request body.");
                }

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckUpdateEndpoint))
            .WithSummary("Update an existing check")
            .WithDescription("Updates an existing check in the accounting system. BankAccountName and BankName are automatically populated from their respective entities based on the provided BankAccountCode and BankId.")
            .Produces<CheckUpdateResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
