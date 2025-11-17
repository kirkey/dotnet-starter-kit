using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Todo.Features.Update.v1;
public static class UpdateTodoEndpoint
{
    internal static RouteHandlerBuilder MapTodoItemUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.
            MapPut("/{id:guid}", async (DefaultIdType id, UpdateTodoCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateTodoEndpoint))
            .WithSummary("Updates a todo item")
            .WithDescription("Updated a todo item")
            .Produces<UpdateTodoResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Todos))
            .MapToApiVersion(new ApiVersion(1, 0));

    }
}
