using FSH.Starter.WebApi.Store.Application.Warehouses.Create.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

public static class CreateWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapCreateWarehouseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateWarehouseCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CreateWarehouseEndpoint))
        .WithSummary("Create a new warehouse")
        .WithDescription("Creates a new warehouse")
        .Produces<CreateWarehouseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Warehouse))
        .MapToApiVersion(1);
    }
}
