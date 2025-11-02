using Accounting.Application.JournalEntries.Lines.Update;

namespace Accounting.Infrastructure.Endpoints.JournalEntryLines.v1;

public static class JournalEntryLineUpdateEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntryLineUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("{id:guid}", async (DefaultIdType id, UpdateJournalEntryLineCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(request);
                return Results.NoContent();
            })
            .WithName(nameof(JournalEntryLineUpdateEndpoint))
            .WithSummary("update journal entry line")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

