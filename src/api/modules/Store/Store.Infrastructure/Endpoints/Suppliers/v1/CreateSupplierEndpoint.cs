using FSH.Starter.WebApi.Store.Application.Suppliers.Create.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class CreateSupplierEndpoint
{
    internal static RouteHandlerBuilder MapCreateSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateSupplierCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateSupplierEndpoint))
            .WithSummary("Create a new supplier")
            .WithDescription("Creates a new supplier")
            .Produces<CreateSupplierResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
            .MapToApiVersion(1);
    }
}
