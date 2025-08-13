using Accounting.Application.Currencies.Dtos;
using Accounting.Application.Currencies.Get;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.Currencies.v1;

public static class CurrencyGetEndpoint
{
    internal static RouteHandlerBuilder MapCurrencyGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCurrencyRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CurrencyGetEndpoint))
            .WithSummary("get a currency by id")
            .WithDescription("get a currency by id")
            .Produces<CurrencyDto>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


