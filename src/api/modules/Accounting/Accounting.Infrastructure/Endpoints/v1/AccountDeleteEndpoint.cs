using FSH.Framework.Infrastructure.Auth.Policy;
using Accounting.Application.Accounts.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.v1;

public static class AccountDeleteEndpoint
{
    internal static RouteHandlerBuilder MapAccountDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new AccountDeleteRequest(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(AccountDeleteEndpoint))
            .WithSummary("deletes account by id")
            .WithDescription("deletes account by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
