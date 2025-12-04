using Accounting.Application.TrialBalance.Create.v1;
using Accounting.Application.TrialBalance.Finalize.v1;
using Accounting.Application.TrialBalance.Get.v1;
using Accounting.Application.TrialBalance.Reopen.v1;
using Accounting.Application.TrialBalance.Search.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.TrialBalance;

/// <summary>
/// Endpoint configuration for Trial Balance module.
/// </summary>
public class TrialBalanceEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/trial-balance").WithTags("trial-balance");

        group.MapPost("/", async (TrialBalanceCreateCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Created($"/accounting/trial-balance/{response.Id}", response);
        })
        .WithName("CreateTrialBalance")
        .WithSummary("Create a new trial balance report")
        .WithDescription("Creates a trial balance report and optionally auto-generates line items from General Ledger")
        .Produces<TrialBalanceCreateResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new TrialBalanceGetRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetTrialBalance")
        .WithSummary("Get trial balance by ID")
        .WithDescription("Retrieves a trial balance report with all line items")
        .Produces<TrialBalanceGetResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/search", async (TrialBalanceSearchRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchTrialBalance")
        .WithSummary("Search trial balance reports")
        .WithDescription("Searches trial balance reports with filtering and pagination")
        .Produces<PagedList<TrialBalanceSearchResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/finalize", async (DefaultIdType id, TrialBalanceFinalizeCommand command, ISender mediator) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID in URL does not match ID in request body.");
            }

            var trialBalanceId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = trialBalanceId, Message = "Trial balance finalized successfully" });
        })
        .WithName("FinalizeTrialBalance")
        .WithSummary("Finalize a trial balance")
        .WithDescription("Finalizes a trial balance report (validates balance and accounting equation)")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reopen", async (DefaultIdType id, TrialBalanceReopenCommand command, ISender mediator) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID in URL does not match ID in request body.");
            }

            var trialBalanceId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = trialBalanceId, Message = "Trial balance reopened successfully" });
        })
        .WithName("ReopenTrialBalance")
        .WithSummary("Reopen a finalized trial balance")
        .WithDescription("Reopens a finalized trial balance to allow modifications")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}
