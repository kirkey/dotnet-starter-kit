using Accounting.Application.Currencies.Delete;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.Currencies.v1;

public static class CurrencyDeleteEndpoint
{
    internal static RouteHandlerBuilder MapCurrencyDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteCurrencyRequest(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(CurrencyDeleteEndpoint))
            .WithSummary("delete currency by id")
            .WithDescription("delete currency by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}


