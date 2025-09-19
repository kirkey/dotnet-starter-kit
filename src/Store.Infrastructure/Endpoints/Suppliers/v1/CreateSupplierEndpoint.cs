using FSH.Starter.WebApi.Store.Application.Suppliers.Create.v1;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class CreateSupplierEndpoint
{
    internal static RouteHandlerBuilder MapCreateSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateSupplierCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/suppliers/{result.SupplierId}", result);
        })
        .WithName("CreateSupplier")
        .WithSummary("Create a new supplier")
        .WithDescription("Creates a new supplier")
        .Produces<CreateSupplierResponse>()
        .MapToApiVersion(1);
    }
}
