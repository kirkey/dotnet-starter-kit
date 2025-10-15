using Accounting.Application.Checks.Create.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for creating/registering a new check.
/// </summary>
public static class CheckCreateEndpoint
{
    internal static RouteHandlerBuilder MapCheckCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CheckCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckCreateEndpoint))
            .WithSummary("Register a new check")
            .WithDescription("Register a new check in the system for later use in payments")
            .Produces<CheckCreateResponse>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
