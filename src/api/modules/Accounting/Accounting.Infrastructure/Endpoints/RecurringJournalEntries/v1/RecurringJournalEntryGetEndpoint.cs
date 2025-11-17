using Accounting.Application.RecurringJournalEntries.Get.v1;
using Accounting.Application.RecurringJournalEntries.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;

public static class RecurringJournalEntryGetEndpoint
{
    internal static RouteHandlerBuilder MapRecurringJournalEntryGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetRecurringJournalEntryRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RecurringJournalEntryGetEndpoint))
            .WithSummary("Get a recurring journal entry template")
            .WithDescription("Get a recurring journal entry template by ID")
            .Produces<RecurringJournalEntryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
