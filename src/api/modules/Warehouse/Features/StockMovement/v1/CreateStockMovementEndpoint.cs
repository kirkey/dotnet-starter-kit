using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.StockMovement.v1;

public static class CreateStockMovementEndpoint
{
    public static void MapStockMovementCreateEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stock-movements", async (CreateStockMovementRequest request, ISender sender) =>
        {
            var response = await sender.Send(new CreateStockMovementCommand(request));
            return Results.Created($"/stock-movements/{response.Id}", response);
        })
        .WithName("CreateStockMovement")
        .WithSummary("Create a new stock movement")
        .WithDescription("Creates a new stock movement for inbound, outbound, or transfer operations")
        .Produces<CreateStockMovementResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

public sealed record CreateStockMovementCommand(CreateStockMovementRequest Request) : IRequest<CreateStockMovementResponse>;

public sealed class CreateStockMovementCommandHandler(CreateStockMovementHandler handler)
    : IRequestHandler<CreateStockMovementCommand, CreateStockMovementResponse>
{
    public async Task<CreateStockMovementResponse> Handle(CreateStockMovementCommand request, CancellationToken cancellationToken)
    {
        return await handler.Handle(request.Request, cancellationToken);
    }
}
