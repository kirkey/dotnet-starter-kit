using Accounting.Application.Currencies.Dtos;
using Accounting.Application.Currencies.Search;
using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.Currencies.v1;

public static class CurrencySearchEndpoint
{
    internal static RouteHandlerBuilder MapCurrencySearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchCurrenciesRequest command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CurrencySearchEndpoint))
            .WithSummary("Gets a list of currencies")
            .WithDescription("Gets a list of currencies with pagination and filtering support")
            .Produces<PagedList<CurrencyDto>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


