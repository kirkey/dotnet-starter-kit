using FSH.Starter.WebApi.Store.Application.PickLists.Create.v1;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class CreatePickListEndpoint
{
    internal static RouteHandlerBuilder MapCreatePickListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePickListCommand request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreatePickListEndpoint))
            .WithSummary("Create a new pick list")
            .WithDescription("Creates a new pick list for warehouse order fulfillment.")
            .Produces<CreatePickListResponse>()
            .RequirePermission("Permissions.Store.Create")
            .MapToApiVersion(1);
    }
}
