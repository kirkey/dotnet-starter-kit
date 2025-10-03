using FSH.Starter.WebApi.Store.Application.SerialNumbers.Update.v1;

namespace Store.Infrastructure.Endpoints.SerialNumbers.v1;

public static class UpdateSerialNumberEndpoint
{
    internal static RouteHandlerBuilder MapUpdateSerialNumberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateSerialNumberCommand request, ISender sender) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("ID mismatch between route and body.");
                }

                var response = await sender.Send(request).ConfigureAwait(false);
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
