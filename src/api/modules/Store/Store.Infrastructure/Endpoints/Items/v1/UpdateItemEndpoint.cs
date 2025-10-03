using FSH.Starter.WebApi.Store.Application.Items.Update.v1;

namespace Store.Infrastructure.Endpoints.Items.v1;

public static class UpdateItemEndpoint
{
    internal static RouteHandlerBuilder MapUpdateItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateItemCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Id mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateItemEndpoint))
            .WithSummary("Update an existing item")
            .WithDescription("Updates an existing inventory item")
            .Produces<UpdateItemResponse>()
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}
