using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.ItemSuppliers.v1;

public static class GetItemSupplierEndpoint
{
    internal static RouteHandlerBuilder MapGetItemSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetItemSupplierCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetItemSupplierEndpoint))
            .WithSummary("Get an item-supplier relationship by ID")
            .WithDescription("Retrieves detailed information about a specific item-supplier relationship")
            .Produces<ItemSupplierResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}
