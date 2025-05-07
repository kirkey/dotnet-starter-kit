using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.App.Features.Update.v1;

public static class GroupUpdateEndpoint
{
    internal static RouteHandlerBuilder MapGroupUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, GroupUpdateCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GroupUpdateEndpoint))
            .WithSummary("Updates a Group Item")
            .WithDescription("Updated a Group Item")
            .Produces<GroupUpdateResponse>(StatusCodes.Status200OK)
            .RequirePermission("Permissions.App.Update")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
