using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Store.Infrastructure.Endpoints.v1;

public class SalesOrdersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("sales-orders").WithTags("Sales Orders");

        group.MapPost("/", async (CreateSalesOrderCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/sales-orders/{result.Id}", result);
        })
        .WithName("CreateSalesOrder")
        .WithSummary("Create a new sales order")
        .WithDescription("Creates a new sales order for retail or wholesale customers");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetSalesOrderQuery(id));
            return Results.Ok(result);
        })
        .WithName("GetSalesOrder")
        .WithSummary("Get sales order by ID")
        .WithDescription("Retrieves a sales order by its unique identifier");

        group.MapPut("/{id:guid}", async (Guid id, UpdateSalesOrderCommand command, ISender sender) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdateSalesOrder")
        .WithSummary("Update sales order")
        .WithDescription("Updates an existing sales order");

        group.MapPost("/{id:guid}/items", async (Guid id, AddSalesOrderItemCommand command, ISender sender) =>
        {
            if (id != command.SalesOrderId)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("AddSalesOrderItem")
        .WithSummary("Add item to sales order")
        .WithDescription("Adds an item to an existing sales order");

        group.MapPut("/{id:guid}/status", async (Guid id, UpdateSalesOrderStatusCommand command, ISender sender) =>
        {
            if (id != command.SalesOrderId)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdateSalesOrderStatus")
        .WithSummary("Update sales order status")
        .WithDescription("Updates the status of a sales order");

        group.MapPost("/search", async (SearchSalesOrdersQuery query, ISender sender) =>
        {
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("SearchSalesOrders")
        .WithSummary("Search sales orders")
        .WithDescription("Search sales orders with filtering and pagination");

        group.MapPost("/{id:guid}/ship", async (Guid id, ShipSalesOrderCommand command, ISender sender) =>
        {
            if (id != command.SalesOrderId)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("ShipSalesOrder")
        .WithSummary("Ship sales order")
        .WithDescription("Ships a sales order and updates inventory");
    }
}
