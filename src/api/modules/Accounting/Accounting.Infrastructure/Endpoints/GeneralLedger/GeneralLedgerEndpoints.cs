using Accounting.Application.GeneralLedgers.Get.v1;
using Accounting.Application.GeneralLedgers.Search.v1;
using Accounting.Application.GeneralLedgers.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.GeneralLedger;

/// <summary>
/// Endpoint configuration for General Ledger module.
/// </summary>
public class GeneralLedgerEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all General Ledger endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/general-ledger").WithTags("general-ledger");

        // Get general ledger entry by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GeneralLedgerGetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetGeneralLedgerEntry")
            .WithSummary("Get general ledger entry by ID")
            .WithDescription("Retrieves a general ledger entry by its unique identifier")
            .Produces<GeneralLedgerGetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Search general ledger entries
        group.MapPost("/search", async (GeneralLedgerSearchRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchGeneralLedgerEntries")
            .WithSummary("Search general ledger entries")
            .WithDescription("Searches general ledger entries with filtering and pagination")
            .Produces<PagedList<GeneralLedgerSearchResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update general ledger entry
        group.MapPut("/{id:guid}", async (DefaultIdType id, GeneralLedgerUpdateCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var entryId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = entryId });
            })
            .WithName("UpdateGeneralLedgerEntry")
            .WithSummary("Update a general ledger entry")
            .WithDescription("Updates general ledger entry details (amounts, memo, USOA class)")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));
    }
}
