using MediatR;
using Store.Application.SalesOrders.Update.v1;

namespace Store.Infrastructure.Endpoints.v1;

public static class AddSalesOrderItemEndpoint
{
    internal static RouteHandlerBuilder MapAddSalesOrderItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/sales-orders/{id:guid}/items", async (Guid id, AddSalesOrderItemCommand command, ISender sender) =>
        {
            if (id != command.SalesOrderId) return Results.BadRequest("ID mismatch");
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("AddSalesOrderItem")
        .WithSummary("Add item to sales order")
        .WithDescription("Adds an item to an existing sales order")
        .MapToApiVersion(1);
    }
}

namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class CreateSalesOrderEndpoint
{
    internal static RouteHandlerBuilder MapCreateSalesOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/sales-orders", async (FSH.Starter.WebApi.Store.Application.SalesOrders.Create.v1.CreateSalesOrderCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/sales-orders/{result.Id}", result);
        })
        .WithName("CreateSalesOrder")
        .WithSummary("Create a new sales order")
        .WithDescription("Creates a new sales order for retail or wholesale customers")
        .MapToApiVersion(1);
    }
}
