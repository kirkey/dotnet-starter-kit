using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.App.Features.Create.v1;

public static class GroupCreateEndpoint
{
    internal static RouteHandlerBuilder MapGroupCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (GroupCreateCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GroupCreateEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(GroupCreateEndpoint))
            .WithSummary("Create a Group Item")
            .WithDescription("Create a Group Item")
            .Produces<GroupCreateResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.App.Create")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
