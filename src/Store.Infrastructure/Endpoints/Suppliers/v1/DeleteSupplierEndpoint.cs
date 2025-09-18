using FSH.Starter.WebApi.Store.Application.Suppliers.Delete.v1;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class DeleteSupplierEndpoint
{
    internal static RouteHandlerBuilder MapDeleteSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteSupplierCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteSupplier")
        .WithSummary("Delete a supplier")
        .WithDescription("Deletes a supplier by id")
        .Produces(StatusCodes.Status204NoContent)
        .MapToApiVersion(1);
    }
}
