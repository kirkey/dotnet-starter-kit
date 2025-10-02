using Accounting.Application.TaxCodes.Get.v1;
using Accounting.Application.TaxCodes.Responses;

namespace Accounting.Infrastructure.Endpoints.TaxCodes.v1;

public static class TaxCodeGetEndpoint
{
    internal static RouteHandlerBuilder MapTaxCodeGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetTaxCodeRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(TaxCodeGetEndpoint))
            .WithSummary("Get a tax code")
            .WithDescription("Get a tax code by ID")
            .Produces<TaxCodeResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
