using Accounting.Application.FiscalPeriodCloses.Commands.v1;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

public static class CompleteTaskEndpoint
{
    internal static RouteHandlerBuilder MapCompleteTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/tasks/complete", async (DefaultIdType id, CompleteTaskCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                    return Results.BadRequest("ID mismatch");
                    
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(CompleteTaskEndpoint))
            .WithSummary("Complete a close task")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

