using Accounting.Application.FiscalPeriodCloses.Commands.v1;
using Accounting.Application.FiscalPeriodCloses.Create.v1;
using Accounting.Application.FiscalPeriodCloses.Get;
using Accounting.Application.FiscalPeriodCloses.Queries;
using Accounting.Application.FiscalPeriodCloses.Responses;
using Accounting.Application.FiscalPeriodCloses.Search;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses;

/// <summary>
/// Endpoint configuration for Fiscal Period Closes module.
/// </summary>
public class FiscalPeriodClosesEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Fiscal Period Closes endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/fiscal-period-closes").WithTags("fiscal-period-closes");

        // Create fiscal period close
        group.MapPost("/", async (FiscalPeriodCloseCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/fiscal-period-closes/{response.Id}", response);
            })
            .WithName("CreateFiscalPeriodClose")
            .WithSummary("Initiate fiscal period close")
            .WithDescription("Initiates a new fiscal period close process (MonthEnd, QuarterEnd, or YearEnd).")
            .Produces<FiscalPeriodCloseCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get fiscal period close by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetFiscalPeriodCloseRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetFiscalPeriodClose")
            .WithSummary("Get fiscal period close by ID with complete details")
            .WithDescription("Returns complete fiscal period close details including tasks, validation status, and audit trail.")
            .Produces<FiscalPeriodCloseDetailsDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Search fiscal period closes
        group.MapPost("/search", async (SearchFiscalPeriodClosesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchFiscalPeriodCloses")
            .WithSummary("Search fiscal period closes")
            .Produces<PagedList<FiscalPeriodCloseResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Complete fiscal period close task
        group.MapPost("/{id:guid}/tasks/complete", async (DefaultIdType id, CompleteFiscalPeriodTaskCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                    return Results.BadRequest("ID mismatch");
                    
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("CompleteFiscalPeriodCloseTask")
            .WithSummary("Complete a fiscal period close task")
            .WithDescription("Marks a task as complete in the fiscal period close checklist")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting));

        // Add validation issue
        group.MapPost("/{id:guid}/validation-issues", async (DefaultIdType id, AddFiscalPeriodCloseValidationIssueCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var closeId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = closeId, Message = "Validation issue added successfully" });
            })
            .WithName("AddFiscalPeriodCloseValidationIssue")
            .WithSummary("Add a validation issue to fiscal period close")
            .WithDescription("Adds a validation issue to the fiscal period close process")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Resolve validation issue
        group.MapPut("/{id:guid}/validation-issues/resolve", async (DefaultIdType id, ResolveFiscalPeriodCloseValidationIssueCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var closeId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = closeId, Message = "Validation issue resolved successfully" });
            })
            .WithName("ResolveFiscalPeriodCloseValidationIssue")
            .WithSummary("Resolve a fiscal period close validation issue")
            .WithDescription("Marks a validation issue as resolved in the fiscal period close process")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Complete fiscal period close
        group.MapPost("/{id:guid}/complete", async (DefaultIdType id, CompleteFiscalPeriodCloseCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                    return Results.BadRequest("ID mismatch");
                    
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("CompleteFiscalPeriodClose")
            .WithSummary("Complete fiscal period close")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting));

        // Reopen fiscal period close
        group.MapPost("/{id:guid}/reopen", async (DefaultIdType id, ReopenFiscalPeriodCloseCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                    return Results.BadRequest("ID mismatch");
                    
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("ReopenFiscalPeriodClose")
            .WithSummary("Reopen fiscal period close")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));
    }
}

