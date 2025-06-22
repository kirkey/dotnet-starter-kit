using Accounting.Application.Payees.Delete.v1;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

public static class PayeeDeleteEndpoint
{
    internal static RouteHandlerBuilder MapPayeeDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new PayeeDeleteCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(PayeeDeleteEndpoint))
            .WithSummary("delete payee by id")
            .WithDescription("delete payee by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
