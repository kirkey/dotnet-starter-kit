using Accounting.Application.TaxCodes.Responses;
using Accounting.Application.TaxCodes.Search.v1;

namespace Accounting.Infrastructure.Endpoints.TaxCodes.v1;

public static class TaxCodeSearchEndpoint
{
    internal static RouteHandlerBuilder MapTaxCodeSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchTaxCodesCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(TaxCodeSearchEndpoint))
            .WithSummary("Search tax codes")
            .WithDescription("Search and filter tax codes with pagination")
            .Produces<PagedList<TaxCodeResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
