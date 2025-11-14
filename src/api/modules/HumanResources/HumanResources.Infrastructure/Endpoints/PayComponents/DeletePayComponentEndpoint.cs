using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents;

public static class DeletePayComponentEndpoint
{
    internal static RouteHandlerBuilder MapDeletePayComponentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeletePayComponentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeletePayComponentEndpoint))
            .WithSummary("Delete a pay component")
            .WithDescription("Deletes a pay component by its unique identifier")
            .Produces<DeletePayComponentResponse>()
            .RequirePermission("Permissions.PayComponents.Delete")
            .MapToApiVersion(1);
    }
}

