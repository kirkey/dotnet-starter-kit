using FSH.Starter.WebApi.Store.Application.Bins.Update.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Bins.v1;

public static class UpdateBinEndpoint
{
    internal static RouteHandlerBuilder MapUpdateBinEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateBinCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBinEndpoint))
            .WithSummary("Update an existing bin")
            .WithDescription("Updates an existing storage bin")
            .Produces<UpdateBinResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
            .MapToApiVersion(1);
    }
}
