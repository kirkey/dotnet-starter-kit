using Accounting.Application.JournalEntries.Create;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

/// <summary>
/// Endpoint for creating a new journal entry with debit and credit lines.
/// </summary>
public static class JournalEntryCreateEndpoint
{
    /// <summary>
    /// Maps the create journal entry endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapJournalEntryCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateJournalEntryCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(JournalEntryGetEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(JournalEntryCreateEndpoint))
            .WithSummary("Create a new journal entry")
            .WithDescription("Create a new journal entry with balanced debit and credit lines for double-entry accounting.")
            .Produces<CreateJournalEntryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
