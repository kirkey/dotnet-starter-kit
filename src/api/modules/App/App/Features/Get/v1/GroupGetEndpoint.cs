using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.App.Features.Get.v1;

public static class GroupGetEndpoint
{
    internal static RouteHandlerBuilder MapGetAppEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAppRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GroupGetEndpoint))
            .WithSummary("gets App group by id")
            .WithDescription("gets App group by id")
            .Produces<GroupGetResponse>()
            .RequirePermission("Permissions.App.View")
            .MapToApiVersion(1);
    }
}
