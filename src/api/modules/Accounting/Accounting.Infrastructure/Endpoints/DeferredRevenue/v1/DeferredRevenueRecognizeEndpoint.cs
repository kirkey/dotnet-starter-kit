using Accounting.Application.DeferredRevenues.Recognize;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenue.v1;

public static class DeferredRevenueRecognizeEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueRecognizeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/recognize", async (DefaultIdType id, RecognizeDeferredRevenueCommand command, ISender mediator) =>
            {
                if (id != command.DeferredRevenueId) return Results.BadRequest("ID in URL does not match ID in request body.");
                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = id, Message = "Deferred revenue recognized successfully" });
            })
            .WithName(nameof(DeferredRevenueRecognizeEndpoint))
            .WithSummary("Recognize deferred revenue")
            .WithDescription("Marks deferred revenue as recognized")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

