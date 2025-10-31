using Accounting.Application.FiscalPeriodCloses.Get;
using Accounting.Application.FiscalPeriodCloses.Responses;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

public static class FiscalPeriodCloseGetEndpoint
{
    internal static RouteHandlerBuilder MapFiscalPeriodCloseGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetFiscalPeriodCloseRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(FiscalPeriodCloseGetEndpoint))
            .WithSummary("Get fiscal period close by ID")
            .Produces<FiscalPeriodCloseResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

