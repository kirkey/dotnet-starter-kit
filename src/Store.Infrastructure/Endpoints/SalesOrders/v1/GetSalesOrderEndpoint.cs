using FSH.Starter.WebApi.Store.Application.SalesOrders.Get.v1;

namespace Store.Infrastructure.Endpoints.SalesOrders.v1;

public static class GetSalesOrderEndpoint
{
    internal static RouteHandlerBuilder MapGetSalesOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetSalesOrderQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetSalesOrder")
        .WithSummary("Get sales order by ID")
        .WithDescription("Retrieves a sales order by its unique identifier")
        .MapToApiVersion(1);
    }
}
