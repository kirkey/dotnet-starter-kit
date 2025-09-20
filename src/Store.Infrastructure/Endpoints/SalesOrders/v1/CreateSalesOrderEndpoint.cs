using FSH.Starter.WebApi.Store.Application.SalesOrders.Create.v1;

namespace Store.Infrastructure.Endpoints.SalesOrders.v1;

public static class CreateSalesOrderEndpoint
{
    internal static RouteHandlerBuilder MapCreateSalesOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateSalesOrderCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CreateSalesOrderEndpoint))
        .WithSummary("Create a new sales order")
        .WithDescription("Creates a new sales order for retail or wholesale customers")
        .Produces<CreateSalesOrderResponse>()
        .RequirePermission("Permissions.Store.Create")
        .MapToApiVersion(1);
    }
}
