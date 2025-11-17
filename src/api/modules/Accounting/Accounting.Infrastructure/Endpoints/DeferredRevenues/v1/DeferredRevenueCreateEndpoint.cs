using Accounting.Application.DeferredRevenues.Create;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenues.v1;

public static class DeferredRevenueCreateEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateDeferredRevenueCommand command, ISender mediator) =>
            {
                var id = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/api/v1/deferred-revenues/{id}", new { Id = id });
            })
            .WithName(nameof(DeferredRevenueCreateEndpoint))
            .WithSummary("Create a new deferred revenue entry")
            .WithDescription("Creates a new deferred revenue entry for revenue recognition tracking")
            .Produces<object>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

