namespace Store.Infrastructure.Endpoints.SalesOrders.v1;

public static class CreateSalesOrderEndpoint
{
    internal static RouteHandlerBuilder MapCreateSalesOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateSalesOrderCommand command, ISender sender) =>
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
