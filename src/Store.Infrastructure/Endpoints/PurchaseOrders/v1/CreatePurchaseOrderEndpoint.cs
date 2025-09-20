using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Create.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for creating a new purchase order.
/// </summary>
public static class CreatePurchaseOrderEndpoint
{
    /// <summary>
    /// Maps the create purchase order endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for create purchase order endpoint</returns>
    internal static RouteHandlerBuilder MapCreatePurchaseOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreatePurchaseOrderCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CreatePurchaseOrderEndpoint))
        .WithSummary("Create a new purchase order")
        .WithDescription("Creates a new purchase order")
        .Produces<CreatePurchaseOrderResponse>()
        .RequirePermission("Permissions.Store.Create")
        .MapToApiVersion(1);
    }
}
