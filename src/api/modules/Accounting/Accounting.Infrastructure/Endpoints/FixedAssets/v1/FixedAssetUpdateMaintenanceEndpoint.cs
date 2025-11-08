using Accounting.Application.FixedAssets.UpdateMaintenance.v1;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetUpdateMaintenanceEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetUpdateMaintenanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/maintenance", async (DefaultIdType id, UpdateMaintenanceCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var assetId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = assetId, Message = "Maintenance schedule updated successfully" });
            })
            .WithName(nameof(FixedAssetUpdateMaintenanceEndpoint))
            .WithSummary("Update maintenance schedule")
            .WithDescription("Updates last and next maintenance dates")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

