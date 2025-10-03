using FSH.Starter.WebApi.Store.Application.LotNumbers.Delete.v1;

namespace Store.Infrastructure.Endpoints.LotNumbers.v1;

public static class DeleteLotNumberEndpoint
{
    internal static RouteHandlerBuilder MapDeleteLotNumberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                await sender.Send(new DeleteLotNumberCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteLotNumberEndpoint))
            .WithSummary("Delete a lot number")
            .WithDescription("Removes a lot number from the system")
            .RequirePermission("Permissions.Store.Delete")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
