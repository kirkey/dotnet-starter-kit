using FSH.Starter.WebApi.Store.Application.SerialNumbers.Update.v1;

namespace Store.Infrastructure.Endpoints.SerialNumbers.v1;

/// <summary>
/// Endpoint for updating a serial number.
/// </summary>
public static class UpdateSerialNumberEndpoint
{
    /// <summary>
    /// Maps the update serial number endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdateSerialNumberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateSerialNumberCommand request, ISender sender) =>
            {
                var command = request with { Id = id };
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateSerialNumberEndpoint))
            .WithSummary("Update a serial number")
            .WithDescription("Updates an existing serial number's status, location, or other properties.")
            .Produces<UpdateSerialNumberResponse>()
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}
