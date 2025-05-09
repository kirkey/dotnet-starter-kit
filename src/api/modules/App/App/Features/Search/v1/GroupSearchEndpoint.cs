using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using FSH.Starter.WebApi.App.Features.Dtos;
using FSH.Starter.WebApi.App.Features.GetList.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.App.Features.Search.v1;

public static class GroupSearchEndpoint
{
    internal static RouteHandlerBuilder MapGroupSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] GroupSearchCommand command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GroupSearchEndpoint))
            .WithSummary("Search a list of Groups")
            .WithDescription("Search a list of Groups with pagination and filtering support")
            .Produces<PagedList<GroupDto>>()
            .RequirePermission("Permissions.App.Search")
            .MapToApiVersion(1);
    }
}
