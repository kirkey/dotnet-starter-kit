using Accounting.Application.JournalEntries.Lines.Get;
using Accounting.Application.JournalEntries.Lines.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.JournalEntryLines.v1;

public static class JournalEntryLineGetEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntryLineGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetJournalEntryLineQuery(id));
                return Results.Ok(response);
            })
            .WithName(nameof(JournalEntryLineGetEndpoint))
            .WithSummary("get journal entry line by id")
            .WithDescription("gets journal entry line by id")
            .Produces<JournalEntryLineResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

