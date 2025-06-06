using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Application.ChartOfAccounts.Get.v1;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.v1;

public static class AccountGetEndpoint
{
    internal static RouteHandlerBuilder MapAccountGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new ChartOfAccountGetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccountGetEndpoint))
            .WithSummary("gets an account by id")
            .WithDescription("gets an account by id")
            .Produces<ChartOfAccountDto>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
