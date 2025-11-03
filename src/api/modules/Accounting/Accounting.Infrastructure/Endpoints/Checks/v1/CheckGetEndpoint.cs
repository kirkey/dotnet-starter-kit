using Accounting.Application.Checks.Get.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for retrieving a single check by ID with complete details.
/// Returns comprehensive check information including status, payment details, and audit trail.
/// </summary>
public static class CheckGetEndpoint
{
    /// <summary>
    /// Maps the check retrieval endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapCheckGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new CheckGetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckGetEndpoint))
            .WithSummary("Get check by ID")
            .WithDescription("Retrieve detailed information about a specific check including current status, payment details, and complete audit trail.")
            .Produces<CheckGetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

