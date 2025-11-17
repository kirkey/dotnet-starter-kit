using FSH.Starter.WebApi.Store.Application.Suppliers.Activate.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class ActivateSupplierEndpoint
{
    internal static RouteHandlerBuilder MapActivateSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/activate", async (DefaultIdType id, ActivateSupplierCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("Supplier ID mismatch");
                }
                
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ActivateSupplierEndpoint))
            .WithSummary("Activate a supplier")
            .WithDescription("Activates a supplier to allow transactions")
            .Produces<ActivateSupplierResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
