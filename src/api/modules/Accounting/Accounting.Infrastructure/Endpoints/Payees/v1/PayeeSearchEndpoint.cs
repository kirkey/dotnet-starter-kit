using Accounting.Application.Payees.Get.v1;
using Accounting.Application.Payees.Search.v1;
using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

public static class PayeeSearchEndpoint
{
    internal static RouteHandlerBuilder MapPayeeSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] PayeeSearchCommand command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PayeeSearchEndpoint))
            .WithSummary("Gets a list of payees")
            .WithDescription("Gets a list of payees with pagination and filtering support")
            .Produces<PagedList<PayeeResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
