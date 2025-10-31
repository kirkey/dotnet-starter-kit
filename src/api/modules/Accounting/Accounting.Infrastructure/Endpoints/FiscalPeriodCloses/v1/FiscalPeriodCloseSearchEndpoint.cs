using Accounting.Application.FiscalPeriodCloses.Responses;
using Accounting.Application.FiscalPeriodCloses.Search;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

public static class FiscalPeriodCloseSearchEndpoint
{
    internal static RouteHandlerBuilder MapFiscalPeriodCloseSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchFiscalPeriodClosesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(FiscalPeriodCloseSearchEndpoint))
            .WithSummary("Search fiscal period closes")
            .Produces<List<FiscalPeriodCloseResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


