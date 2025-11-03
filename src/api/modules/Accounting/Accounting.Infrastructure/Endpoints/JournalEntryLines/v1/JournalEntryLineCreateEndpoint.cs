using Accounting.Application.JournalEntries.Lines.Create;

namespace Accounting.Infrastructure.Endpoints.JournalEntryLines.v1;

public static class JournalEntryLineCreateEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntryLineCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost(string.Empty, async (CreateJournalEntryLineCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request);
                return Results.Ok(id);
            })
            .WithName(nameof(JournalEntryLineCreateEndpoint))
            .WithSummary("create journal entry line")
            .WithDescription("creates a new journal entry line")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

