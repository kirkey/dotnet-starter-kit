using Accounting.Application.FiscalPeriodCloses.Commands.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

public static class ReopenFiscalPeriodCloseEndpoint
{
    internal static RouteHandlerBuilder MapReopenFiscalPeriodCloseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reopen", async (DefaultIdType id, ReopenFiscalPeriodCloseCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                    return Results.BadRequest("ID mismatch");
                    
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(ReopenFiscalPeriodCloseEndpoint))
            .WithSummary("Reopen fiscal period close")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

