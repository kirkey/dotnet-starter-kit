using Accounting.Application.RecurringJournalEntries.Generate.v1;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;

public static class RecurringJournalEntryGenerateEndpoint
{
    internal static RouteHandlerBuilder MapRecurringJournalEntryGenerateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/generate", async (DefaultIdType id, GenerateRecurringJournalEntryCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var journalEntryId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { JournalEntryId = journalEntryId, Message = "Journal entry generated successfully" });
            })
            .WithName(nameof(RecurringJournalEntryGenerateEndpoint))
            .WithSummary("Generate journal entry from template")
            .WithDescription("Manually generates a journal entry from a recurring template")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

