using Accounting.Application.JournalEntries.Update;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

/// <summary>
/// Endpoint for updating an unposted journal entry.
/// </summary>
public static class JournalEntryUpdateEndpoint
{
    /// <summary>
    /// Maps the update journal entry endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapJournalEntryUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateJournalEntryCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(JournalEntryUpdateEndpoint))
            .WithSummary("Update an unposted journal entry")
            .WithDescription("Update the details of an unposted journal entry. Posted entries cannot be modified - use reverse instead.")
            .Produces<UpdateJournalEntryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

