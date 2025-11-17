using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Todo.Features.Get.v1;
public static class GetTodoEndpoint
{
    internal static RouteHandlerBuilder MapGetTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
                        {
                            var response = await mediator.Send(new GetTodoRequest(id)).ConfigureAwait(false);
                            return Results.Ok(response);
                        })
                        .WithName(nameof(GetTodoEndpoint))
                        .WithSummary("gets todo item by id")
                        .WithDescription("gets todo item by id")
                        .Produces<GetTodoResponse>()
                        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Todos))
                        .MapToApiVersion(1);
    }
}
