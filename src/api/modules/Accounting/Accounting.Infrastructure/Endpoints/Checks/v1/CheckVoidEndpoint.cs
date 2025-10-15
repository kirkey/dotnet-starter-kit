using Accounting.Application.Checks.Void.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for voiding a check.
/// </summary>
public static class CheckVoidEndpoint
{
    internal static RouteHandlerBuilder MapCheckVoidEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/void", async (CheckVoidCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckVoidEndpoint))
            .WithSummary("Void a check")
            .WithDescription("Void a check that was issued in error or needs to be cancelled")
            .Produces<CheckVoidResponse>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

