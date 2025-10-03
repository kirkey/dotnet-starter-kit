using FSH.Starter.WebApi.Store.Application.SerialNumbers.Create.v1;

namespace Store.Infrastructure.Endpoints.SerialNumbers.v1;

public static class CreateSerialNumberEndpoint
{
    internal static RouteHandlerBuilder MapCreateSerialNumberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateSerialNumberCommand request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateSerialNumberEndpoint))
            .WithSummary("Create a new serial number")
            .WithDescription("Creates a new serial number for unit-level tracking of inventory items.")
            .Produces<CreateSerialNumberResponse>()
            .RequirePermission("Permissions.Store.Create")
            .MapToApiVersion(1);
    }
}
