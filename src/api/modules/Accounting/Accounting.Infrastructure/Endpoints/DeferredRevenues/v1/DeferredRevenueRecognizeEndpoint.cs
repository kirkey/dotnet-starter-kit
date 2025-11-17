using Accounting.Application.DeferredRevenues.Recognize;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenues.v1;

public static class DeferredRevenueRecognizeEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueRecognizeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/recognize", async (DefaultIdType id, RecognizeDeferredRevenueCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = result, Message = "Deferred revenue recognized successfully" });
            })
            .WithName(nameof(DeferredRevenueRecognizeEndpoint))
            .WithSummary("Recognize deferred revenue")
            .WithDescription("Marks deferred revenue as recognized, preventing further modifications")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

