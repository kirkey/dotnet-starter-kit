using Accounting.Application.JournalEntries.Lines.Delete;

namespace Accounting.Infrastructure.Endpoints.JournalEntryLines.v1;

public static class JournalEntryLineDeleteEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntryLineDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteJournalEntryLineCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(JournalEntryLineDeleteEndpoint))
            .WithSummary("delete journal entry line")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}

