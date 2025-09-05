using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Delete.v1;

public static class DeleteWarehouseEndpoint
{
    public static void MapWarehouseDeleteEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new DeleteWarehouseCommand(id));
            return Results.Ok(response);
        })
        .WithName("DeleteWarehouse")
        .WithSummary("Delete (deactivate) warehouse")
        .WithDescription("Deactivates a warehouse instead of hard deletion to preserve data integrity")
        .Produces<DeleteWarehouseResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

public sealed record DeleteWarehouseCommand(DefaultIdType Id) : IRequest<DeleteWarehouseResponse>;

public sealed class DeleteWarehouseCommandHandler(DeleteWarehouseHandler handler)
    : IRequestHandler<DeleteWarehouseCommand, DeleteWarehouseResponse>
{
    public async Task<DeleteWarehouseResponse> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        return await handler.Handle(new DeleteWarehouseRequest(request.Id), cancellationToken);
    }
}
