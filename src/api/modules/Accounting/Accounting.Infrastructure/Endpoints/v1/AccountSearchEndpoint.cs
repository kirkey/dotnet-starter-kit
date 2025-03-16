using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using Accounting.Application.Accounts.Get.v1;
using Accounting.Application.Accounts.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.v1;

public static class AccountSearchEndpoint
{
    internal static RouteHandlerBuilder MapAccountSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] AccountSearchRequest command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccountSearchEndpoint))
            .WithSummary("Gets a list of accounts")
            .WithDescription("Gets a list of accounts with pagination and filtering support")
            .Produces<PagedList<AccountResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
