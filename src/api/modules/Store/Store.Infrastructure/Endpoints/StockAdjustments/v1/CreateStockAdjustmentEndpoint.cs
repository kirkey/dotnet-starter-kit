using FSH.Starter.WebApi.Store.Application.StockAdjustments.Create.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.StockAdjustments.v1;

public static class CreateStockAdjustmentEndpoint
{
    internal static RouteHandlerBuilder MapCreateStockAdjustmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateStockAdjustmentCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CreateStockAdjustmentEndpoint))
        .WithSummary("Create a new stock adjustment")
        .WithDescription("Creates a stock adjustment for inventory")
        .Produces<CreateStockAdjustmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);
    }
}
