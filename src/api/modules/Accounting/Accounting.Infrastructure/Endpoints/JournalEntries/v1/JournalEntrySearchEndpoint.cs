using Accounting.Application.JournalEntries.Dtos;
using Accounting.Application.JournalEntries.Search;
using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

public static class JournalEntrySearchEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntrySearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchJournalEntriesRequest command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(JournalEntrySearchEndpoint))
            .WithSummary("Gets a list of journal entries")
            .WithDescription("Gets a list of journal entries with pagination and filtering support")
            .Produces<PagedList<JournalEntryDto>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


