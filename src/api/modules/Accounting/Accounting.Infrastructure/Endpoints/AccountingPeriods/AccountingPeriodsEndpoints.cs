using Accounting.Application.AccountingPeriods.Close.v1;
using Accounting.Application.AccountingPeriods.Create.v1;
using Accounting.Application.AccountingPeriods.Delete.v1;
using Accounting.Application.AccountingPeriods.Get.v1;
using Accounting.Application.AccountingPeriods.Reopen.v1;
using Accounting.Application.AccountingPeriods.Responses;
using Accounting.Application.AccountingPeriods.Search.v1;
using Accounting.Application.AccountingPeriods.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods;

/// <summary>
/// Endpoint configuration for Accounting Periods module.
/// </summary>
public class AccountingPeriodsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Accounting Periods endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/accounting-periods").WithTags("accounting-periods");

        group.MapPost("/", async (CreateAccountingPeriodCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName("CreateAccountingPeriod")
            .WithSummary("create accounting period")
            .WithDescription("create accounting period")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAccountingPeriodQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetAccountingPeriod")
            .WithSummary("get accounting period by id")
            .WithDescription("get accounting period by id")
            .Produces<AccountingPeriodResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateAccountingPeriodCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateAccountingPeriod")
            .WithSummary("update accounting period")
            .WithDescription("update accounting period")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteAccountingPeriodCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteAccountingPeriod")
            .WithSummary("delete accounting period by id")
            .WithDescription("delete accounting period by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        group.MapPost("/search", async (ISender mediator, [FromBody] SearchAccountingPeriodsRequest request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchAccountingPeriods")
            .WithSummary("search accounting periods")
            .WithDescription("search accounting periods")
            .Produces<PagedList<AccountingPeriodResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        group.MapPost("/{id:guid}/close", async (DefaultIdType id, AccountingPeriodCloseCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var response = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(response);
            })
            .WithName("CloseAccountingPeriod")
            .WithSummary("Close accounting period")
            .WithDescription("Closes an accounting period")
            .Produces<AccountingPeriodTransitionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting));

        group.MapPost("/{id:guid}/reopen", async (DefaultIdType id, AccountingPeriodReopenCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var response = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(response);
            })
            .WithName("ReopenAccountingPeriod")
            .WithSummary("Reopen accounting period")
            .WithDescription("Reopens a previously closed accounting period")
            .Produces<AccountingPeriodTransitionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));
    }
}
