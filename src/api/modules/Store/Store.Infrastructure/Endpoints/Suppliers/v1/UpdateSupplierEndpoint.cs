using FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

/// <summary>
/// Endpoint for updating a supplier.
/// </summary>
public static class UpdateSupplierEndpoint
{
    /// <summary>
    /// Maps the update supplier endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdateSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateSupplierCommand request, ISender sender) =>
        {
            var command = request with { Id = id };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(UpdateSupplierEndpoint))
        .WithSummary("Update a supplier")
        .WithDescription("Updates an existing supplier")
        .Produces<UpdateSupplierResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);
    }
}
