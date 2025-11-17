using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Todo.Features.GetList.v1;

public static class GetTodoListEndpoint
{
    internal static RouteHandlerBuilder MapGetTodoListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (ISender mediator, [FromBody] PaginationFilter filter) =>
        {
            var response = await mediator.Send(new GetTodoListRequest(filter)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetTodoListEndpoint))
        .WithSummary("Gets a list of todo items with paging support")
        .WithDescription("Gets a list of todo items with paging support")
        .Produces<PagedList<TodoDto>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Todos))
        .MapToApiVersion(1);
    }
}
