using FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.SerialNumbers.v1;

public static class GetSerialNumberEndpoint
{
    internal static RouteHandlerBuilder MapGetSerialNumberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetSerialNumberCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetSerialNumberEndpoint))
            .WithSummary("Get a serial number by ID")
            .WithDescription("Retrieves a specific serial number by its unique identifier.")
            .Produces<SerialNumberResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}
