using Accounting.Application.FiscalPeriodCloses.Commands.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

public static class CompleteFiscalPeriodCloseTaskEndpoint
{
    internal static RouteHandlerBuilder MapCompleteFiscalPeriodCloseTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/tasks/complete", async (DefaultIdType id, CompleteFiscalPeriodTaskCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                    return Results.BadRequest("ID mismatch");
                    
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(CompleteFiscalPeriodCloseTaskEndpoint))
            .WithSummary("Complete a fiscal period close task")
            .WithDescription("Marks a task as complete in the fiscal period close checklist")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

