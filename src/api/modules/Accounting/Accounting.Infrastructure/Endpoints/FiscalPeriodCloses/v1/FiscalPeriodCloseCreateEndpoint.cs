using Accounting.Application.FiscalPeriodCloses.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

public static class FiscalPeriodCloseCreateEndpoint
{
    internal static RouteHandlerBuilder MapFiscalPeriodCloseCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateFiscalPeriodCloseCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/fiscal-period-closes/{response.Id}", response);
            })
            .WithName(nameof(FiscalPeriodCloseCreateEndpoint))
            .WithSummary("Initiate fiscal period close")
            .WithDescription("Initiates a new fiscal period close process (MonthEnd, QuarterEnd, or YearEnd).")
            .Produces<FiscalPeriodCloseCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

