using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Delete.v1;
using Shared.Authorization;

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
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
            .MapToApiVersion(1);
    }
}
