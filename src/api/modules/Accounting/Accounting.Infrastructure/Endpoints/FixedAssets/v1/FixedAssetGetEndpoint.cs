using Accounting.Application.FixedAssets.Dtos;
using Accounting.Application.FixedAssets.Get;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.FixedAssets.v1;

public static class FixedAssetGetEndpoint
{
    internal static RouteHandlerBuilder MapFixedAssetGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetFixedAssetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(FixedAssetGetEndpoint))
            .WithSummary("get a fixed asset by id")
            .WithDescription("get a fixed asset by id")
            .Produces<FixedAssetDto>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


