using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.StockMovement.v1;

public static class ConfirmStockMovementEndpoint
{
    public static void MapConfirmStockMovementEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/stock-movements/{id:guid}/confirm", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new ConfirmStockMovementCommand(id));
            return Results.Ok(response);
        })
        .WithName("ConfirmStockMovement")
        .WithSummary("Confirm stock movement")
        .WithDescription("Confirms a pending stock movement and updates inventory accordingly")
        .Produces<ConfirmStockMovementResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

public sealed record ConfirmStockMovementCommand(DefaultIdType Id) : IRequest<ConfirmStockMovementResponse>;

public sealed class ConfirmStockMovementCommandHandler(ConfirmStockMovementHandler handler)
    : IRequestHandler<ConfirmStockMovementCommand, ConfirmStockMovementResponse>
{
    public async Task<ConfirmStockMovementResponse> Handle(ConfirmStockMovementCommand request, CancellationToken cancellationToken)
    {
        return await handler.Handle(new ConfirmStockMovementRequest(request.Id), cancellationToken);
    }
}
