using FSH.Starter.WebApi.Store.Application.Bins.Create.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Bins.v1;

public static class CreateBinEndpoint
{
    internal static RouteHandlerBuilder MapCreateBinEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBinCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateBinEndpoint))
            .WithSummary("Create a new bin")
            .WithDescription("Creates a new storage bin within a warehouse location")
            .Produces<CreateBinResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Warehouse))
            .MapToApiVersion(1);
    }
}
