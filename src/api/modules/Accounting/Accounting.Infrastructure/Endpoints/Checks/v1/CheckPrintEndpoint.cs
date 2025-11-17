using Accounting.Application.Checks.Print.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for marking a check as printed.
/// Records print status and who performed the printing for audit trail purposes.
/// </summary>
public static class CheckPrintEndpoint
{
    /// <summary>
    /// Maps the check print endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapCheckPrintEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/print", async (CheckPrintCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckPrintEndpoint))
            .WithSummary("Mark check as printed")
            .WithDescription("Mark a check as printed and record who performed the printing. Maintains audit trail for compliance.")
            .Produces<CheckPrintResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

