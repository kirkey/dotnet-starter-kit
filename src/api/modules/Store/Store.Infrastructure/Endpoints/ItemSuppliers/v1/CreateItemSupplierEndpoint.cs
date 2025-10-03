using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Create.v1;

namespace Store.Infrastructure.Endpoints.ItemSuppliers.v1;

public static class CreateItemSupplierEndpoint
{
    internal static RouteHandlerBuilder MapCreateItemSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateItemSupplierCommand request, ISender sender) =>
            {
                var response = await sender.Send(request);
                return Results.Created($"/api/v1/store/itemsuppliers/{response.Id}", response);
            })
            .WithName(nameof(CreateItemSupplierEndpoint))
            .WithSummary("Create a new item-supplier relationship")
            .WithDescription("Creates a new relationship between an item and a supplier with pricing and lead time details")
            .RequirePermission("Permissions.Store.Create")
            .Produces<CreateItemSupplierResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .MapToApiVersion(1);
    }
}
