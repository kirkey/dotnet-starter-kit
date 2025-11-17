using Accounting.Application.FiscalPeriodCloses.Get;
using Accounting.Application.FiscalPeriodCloses.Queries;
using Shared.Authorization;

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
            .WithSummary("Get fiscal period close by ID with complete details")
            .WithDescription("Returns complete fiscal period close details including tasks, validation status, and audit trail.")
            .Produces<FiscalPeriodCloseDetailsDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

