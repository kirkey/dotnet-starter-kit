using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.App.Features.Delete.v1;

public static class GroupDeleteEndpoint
{
    internal static RouteHandlerBuilder MapGroupDeletionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new GroupDeleteCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(GroupDeleteEndpoint))
            .WithSummary("Deletes a Group Item")
            .WithDescription("Deleted a Group Item")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.App.Delete")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
