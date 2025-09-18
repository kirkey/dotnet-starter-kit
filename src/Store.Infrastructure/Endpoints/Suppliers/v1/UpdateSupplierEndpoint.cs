using FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;
using FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class UpdateSupplierEndpoint
{
    internal static RouteHandlerBuilder MapUpdateSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateSupplierCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateSupplier")
        .WithSummary("Update a supplier")
        .WithDescription("Updates an existing supplier")
        .Produces<UpdateSupplierResponse>()
        .MapToApiVersion(1);
    }
}
