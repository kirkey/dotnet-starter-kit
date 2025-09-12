namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class GetSalesOrderEndpoint
{
    internal static RouteHandlerBuilder MapGetSalesOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/sales-orders/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new FSH.Starter.WebApi.Store.Application.SalesOrders.Get.v1.GetSalesOrderQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetSalesOrder")
        .WithSummary("Get sales order by ID")
        .WithDescription("Retrieves a sales order by its unique identifier")
        .MapToApiVersion(1);
    }
}

