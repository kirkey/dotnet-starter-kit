using FSH.Framework.Infrastructure.Auth.Policy;
using Accounting.Application.Accounts.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.v1;

public static class UpdateAccountEndpoint
{
    internal static RouteHandlerBuilder MapAccountUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateAccountCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateAccountEndpoint))
            .WithSummary("update an account")
            .WithDescription("update an account")
            .Produces<UpdateAccountResponse>()
            .RequirePermission("Permissions.Accounts.Update")
            .MapToApiVersion(1);
    }
}
