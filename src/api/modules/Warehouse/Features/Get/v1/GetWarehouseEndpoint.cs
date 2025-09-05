using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Get.v1;

public static class GetWarehouseEndpoint
{
    public static void MapGetWarehouseEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetWarehouseQuery(id));
            return Results.Ok(response);
        })
        .WithName("GetWarehouse")
        .WithSummary("Get warehouse by ID")
        .WithDescription("Retrieves a warehouse by its unique identifier")
        .Produces<GetWarehouseResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}

public sealed record GetWarehouseQuery(DefaultIdType Id) : IRequest<GetWarehouseResponse>;

public sealed class GetWarehouseQueryHandler(GetWarehouseHandler handler)
    : IRequestHandler<GetWarehouseQuery, GetWarehouseResponse>
{
    public async Task<GetWarehouseResponse> Handle(GetWarehouseQuery request, CancellationToken cancellationToken)
    {
        return await handler.Handle(new GetWarehouseRequest(request.Id), cancellationToken);
    }
}
