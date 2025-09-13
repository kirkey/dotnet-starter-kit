using FSH.Starter.WebApi.Store.Application.SalesOrders.Delete.v1;

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
