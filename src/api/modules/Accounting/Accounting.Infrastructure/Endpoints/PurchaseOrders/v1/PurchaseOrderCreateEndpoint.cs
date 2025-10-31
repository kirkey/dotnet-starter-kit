using Accounting.Application.PurchaseOrders.Create.v1;

namespace Accounting.Infrastructure.Endpoints.PurchaseOrders.v1;

public static class PurchaseOrderCreateEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseOrderCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (PurchaseOrderCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/purchase-orders/{response.Id}", response);
            })
            .WithName(nameof(PurchaseOrderCreateEndpoint))
            .WithSummary("Create purchase order")
            .Produces<PurchaseOrderCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

