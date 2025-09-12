using Store.Application.SalesOrders.Delete.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.SalesOrders.v1;

public static class DeleteSalesOrderEndpoint
{
    internal static RouteHandlerBuilder MapDeleteSalesOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteSalesOrderCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteSalesOrder")
        .WithSummary("Delete sales order")
        .WithDescription("Deletes a sales order by its unique identifier")
        .MapToApiVersion(1);
    }
}
using Store.Application.SalesOrders.Create.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.SalesOrders.v1;

public static class CreateSalesOrderEndpoint
{
    internal static RouteHandlerBuilder MapCreateSalesOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/sales-orders", async (CreateSalesOrderCommand command, ISender sender) =>
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
