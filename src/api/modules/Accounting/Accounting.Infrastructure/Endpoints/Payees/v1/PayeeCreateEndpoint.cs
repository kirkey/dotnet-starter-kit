using Accounting.Application.Payees.Create.v1;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

public static class PayeeCreateEndpoint
{
    internal static RouteHandlerBuilder MapPayeeCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (PayeeCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PayeeCreateEndpoint))
            .WithSummary("create a payee")
            .WithDescription("create a payee")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
