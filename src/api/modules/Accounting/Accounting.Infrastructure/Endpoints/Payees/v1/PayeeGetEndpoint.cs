using Accounting.Application.Payees.Get.v1;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

public static class PayeeGetEndpoint
{
    internal static RouteHandlerBuilder MapPayeeGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new PayeeGetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PayeeGetEndpoint))
            .WithSummary("get a payee by id")
            .WithDescription("get a payee by id")
            .Produces<PayeeResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
