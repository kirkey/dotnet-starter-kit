using Accounting.Application.Checks.Get.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for retrieving a single check by ID.
/// </summary>
public static class CheckGetEndpoint
{
    internal static RouteHandlerBuilder MapCheckGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new CheckGetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckGetEndpoint))
            .WithSummary("Get check by ID")
            .WithDescription("Retrieve detailed information about a specific check")
            .Produces<CheckGetResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

