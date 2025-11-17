using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Update.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.ItemSuppliers.v1;

public static class UpdateItemSupplierEndpoint
{
    internal static RouteHandlerBuilder MapUpdateItemSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateItemSupplierCommand request, ISender sender) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body");
                }

                var response = await sender.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateItemSupplierEndpoint))
            .WithSummary("Update an item-supplier relationship")
            .WithDescription("Updates pricing, lead time, and other details for an existing item-supplier relationship")
            .Produces<UpdateItemSupplierResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
