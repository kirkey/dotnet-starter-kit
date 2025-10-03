using FSH.Starter.WebApi.Store.Application.Bins.Get.v1;

namespace Store.Infrastructure.Endpoints.Bins.v1;

public static class GetBinEndpoint
{
    internal static RouteHandlerBuilder MapGetBinEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBinRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetBinEndpoint))
            .WithSummary("Get bin by ID")
            .WithDescription("Retrieves a specific storage bin by its ID")
            .Produces<BinResponse>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
