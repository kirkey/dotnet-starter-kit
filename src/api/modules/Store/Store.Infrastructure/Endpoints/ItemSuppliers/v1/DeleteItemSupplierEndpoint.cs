using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Delete.v1;

namespace Store.Infrastructure.Endpoints.ItemSuppliers.v1;

public static class DeleteItemSupplierEndpoint
{
    internal static RouteHandlerBuilder MapDeleteItemSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                await sender.Send(new DeleteItemSupplierCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteItemSupplierEndpoint))
            .WithSummary("Delete an item-supplier relationship")
            .WithDescription("Removes an item-supplier relationship from the system")
            .RequirePermission("Permissions.Store.Delete")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
