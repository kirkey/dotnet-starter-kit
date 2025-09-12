namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class UpdateSalesOrderEndpoint
{
    internal static RouteHandlerBuilder MapUpdateSalesOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/sales-orders/{id:guid}", async (Guid id, FSH.Starter.WebApi.Store.Application.SalesOrders.Update.v1.UpdateSalesOrderCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateSalesOrder")
        .WithSummary("Update sales order")
        .WithDescription("Updates an existing sales order")
        .MapToApiVersion(1);
    }
}

