using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Todo.Features.Delete.v1;
public static class DeleteTodoEndpoint
{
    internal static RouteHandlerBuilder MapTodoItemDeletionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteTodoCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(DeleteTodoEndpoint))
            .WithSummary("Deletes a todo item")
            .WithDescription("Deleted a todo item")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Todos))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
