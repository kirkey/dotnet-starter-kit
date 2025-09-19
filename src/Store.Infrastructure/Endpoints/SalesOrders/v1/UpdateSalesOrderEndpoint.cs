using FSH.Starter.WebApi.Store.Application.SalesOrders.Update.v1;

namespace Store.Infrastructure.Endpoints.SalesOrders.v1;

public static class UpdateSalesOrderEndpoint
{
    internal static RouteHandlerBuilder MapUpdateSalesOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateSalesOrderCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateSalesOrder")
        .WithSummary("Update sales order")
        .WithDescription("Updates an existing sales order")
        .Produces<UpdateSalesOrderResponse>()
        .MapToApiVersion(1);
    }
}
