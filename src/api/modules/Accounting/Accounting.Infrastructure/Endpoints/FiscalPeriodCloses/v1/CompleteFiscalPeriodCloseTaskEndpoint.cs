using Accounting.Application.FiscalPeriodCloses.Commands.v1;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

public static class CompleteFiscalPeriodCloseTaskEndpoint
{
    internal static RouteHandlerBuilder MapCompleteFiscalPeriodCloseTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/tasks/complete", async (DefaultIdType id, CompleteFiscalPeriodCloseCommand command, ISender mediator) =>
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
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

