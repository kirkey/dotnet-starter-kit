using Accounting.Application.RecurringJournalEntries.Create.v1;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;

public static class RecurringJournalEntryCreateEndpoint
{
    internal static RouteHandlerBuilder MapRecurringJournalEntryCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateRecurringJournalEntryCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/recurring-journal-entries/{response}", response);
            })
            .WithName(nameof(RecurringJournalEntryCreateEndpoint))
            .WithSummary("Create a recurring journal entry template")
            .WithDescription("Create a new recurring journal entry template")
            .Produces<DefaultIdType>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
