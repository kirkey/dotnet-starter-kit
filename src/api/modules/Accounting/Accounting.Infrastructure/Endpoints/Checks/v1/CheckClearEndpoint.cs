using Accounting.Application.Checks.Clear.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for marking a check as cleared.
/// </summary>
public static class CheckClearEndpoint
{
    internal static RouteHandlerBuilder MapCheckClearEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/clear", async (CheckClearCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckClearEndpoint))
            .WithSummary("Mark check as cleared")
            .WithDescription("Mark a check as cleared through bank reconciliation")
            .Produces<CheckClearResponse>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

