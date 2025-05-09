using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.App.Features.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.App.Features.GetList.v1;

public static class GroupGetListEndpoint
{
    internal static RouteHandlerBuilder MapGroupGetListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/list", async (ISender mediator, [FromBody] PaginationFilter filter) =>
            {
                var response = await mediator.Send(new GroupGetListRequest(filter)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GroupGetListEndpoint))
            .WithSummary("Gets a list of App items with paging support")
            .WithDescription("Gets a list of App items with paging support")
            .Produces<PagedList<GroupDto>>()
            .RequirePermission("Permissions.App.View")
            .MapToApiVersion(1);
    }
}
