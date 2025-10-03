using FSH.Starter.WebApi.Store.Application.PickLists.Get.v1;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class GetPickListEndpoint
{
    internal static RouteHandlerBuilder MapGetPickListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var request = new GetPickListRequest { PickListId = id };
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetPickListEndpoint))
            .WithSummary("Get pick list by ID")
            .WithDescription("Retrieves a specific pick list with all items.")
            .Produces<GetPickListResponse>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
