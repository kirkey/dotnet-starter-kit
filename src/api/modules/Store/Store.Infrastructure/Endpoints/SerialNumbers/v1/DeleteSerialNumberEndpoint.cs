using FSH.Starter.WebApi.Store.Application.SerialNumbers.Delete.v1;

namespace Store.Infrastructure.Endpoints.SerialNumbers.v1;

public static class DeleteSerialNumberEndpoint
{
    internal static RouteHandlerBuilder MapDeleteSerialNumberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new DeleteSerialNumberCommand { Id = id }).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteSerialNumberEndpoint))
            .WithSummary("Delete a serial number")
            .WithDescription("Deletes an existing serial number from the system.")
            .Produces<DeleteSerialNumberResponse>()
            .RequirePermission("Permissions.Store.Delete")
            .MapToApiVersion(1);
    }
}
