using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Create.v1;

public static class CreateWarehouseEndpoint
{
    public static void MapWarehouseCreateEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateWarehouseRequest request, ISender sender) =>
        {
            var response = await sender.Send(new CreateWarehouseCommand(request));
            return Results.Created($"/warehouses/{response.Id}", response);
        })
        .WithName("CreateWarehouse")
        .WithSummary("Create a new warehouse")
        .WithDescription("Creates a new warehouse with the specified details")
        .Produces<CreateWarehouseResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}

public sealed record CreateWarehouseCommand(CreateWarehouseRequest Request) : IRequest<CreateWarehouseResponse>
{
    public CreateWarehouseHandler Handler { get; } = default!;
}

public sealed class CreateWarehouseCommandHandler(CreateWarehouseHandler handler)
    : IRequestHandler<CreateWarehouseCommand, CreateWarehouseResponse>
{
    public async Task<CreateWarehouseResponse> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        return await handler.Handle(request.Request, cancellationToken);
    }
}
