using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Update.v1;

public static class UpdateWarehouseEndpoint
{
    public static void MapWarehouseUpdateEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWarehouseRequest request, ISender sender) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("ID in URL does not match ID in request body.");
            }

            var response = await sender.Send(new UpdateWarehouseCommand(request));
            return Results.Ok(response);
        })
        .WithName("UpdateWarehouse")
        .WithSummary("Update warehouse")
        .WithDescription("Updates an existing warehouse with the specified details")
        .Produces<UpdateWarehouseResponse>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

public sealed record UpdateWarehouseCommand(UpdateWarehouseRequest Request) : IRequest<UpdateWarehouseResponse>;

public sealed class UpdateWarehouseCommandHandler(UpdateWarehouseHandler handler)
    : IRequestHandler<UpdateWarehouseCommand, UpdateWarehouseResponse>
{
    public async Task<UpdateWarehouseResponse> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        return await handler.Handle(request.Request, cancellationToken);
    }
}
