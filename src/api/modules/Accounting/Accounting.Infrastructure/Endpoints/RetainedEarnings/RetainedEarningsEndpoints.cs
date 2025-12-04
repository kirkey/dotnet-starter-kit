using Accounting.Application.RetainedEarnings.Close.v1;
using Accounting.Application.RetainedEarnings.Create.v1;
using Accounting.Application.RetainedEarnings.Get;
using Accounting.Application.RetainedEarnings.RecordDistribution.v1;
using Accounting.Application.RetainedEarnings.Reopen.v1;
using Accounting.Application.RetainedEarnings.Responses;
using Accounting.Application.RetainedEarnings.Search.v1;
using Accounting.Application.RetainedEarnings.UpdateNetIncome.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings;

/// <summary>
/// Endpoint configuration for Retained Earnings module.
/// Provides comprehensive REST API endpoints for managing retained earnings.
/// </summary>
public class RetainedEarningsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Retained Earnings endpoints to the route builder.
    /// Includes CRUD and workflow operations.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/retained-earnings").WithTags("retained-earnings");

        // Create endpoint
        group.MapPost("/", async (RetainedEarningsCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/retained-earnings/{response.Id}", response);
            })
            .WithName("CreateRetainedEarnings")
            .WithSummary("Create retained earnings record")
            .Produces<RetainedEarningsCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetRetainedEarningsRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetRetainedEarnings")
            .WithSummary("Get retained earnings details by ID")
            .Produces<RetainedEarningsDetailsResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async (SearchRetainedEarningsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchRetainedEarnings")
            .WithSummary("Search retained earnings")
            .Produces<List<RetainedEarningsResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update net income endpoint
        group.MapPut("/{id:guid}/net-income", async (DefaultIdType id, UpdateNetIncomeCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId, Message = "Net income updated successfully" });
            })
            .WithName("UpdateRetainedEarningsNetIncome")
            .WithSummary("Update net income")
            .WithDescription("Updates the net income for the fiscal year")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Record distribution endpoint
        group.MapPost("/{id:guid}/distributions", async (DefaultIdType id, RecordDistributionCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId, Message = "Distribution recorded successfully" });
            })
            .WithName("RecordRetainedEarningsDistribution")
            .WithSummary("Record distribution")
            .WithDescription("Records a distribution to members or shareholders")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
            .MapToApiVersion(1);

        // Close endpoint
        group.MapPost("/{id:guid}/close", async (DefaultIdType id, CloseRetainedEarningsCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId, Message = "Fiscal year closed successfully" });
            })
            .WithName("CloseRetainedEarnings")
            .WithSummary("Close fiscal year")
            .WithDescription("Closes the fiscal year for retained earnings")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting))
            .MapToApiVersion(1);

        // Reopen endpoint
        group.MapPost("/{id:guid}/reopen", async (DefaultIdType id, ReopenRetainedEarningsCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId, Message = "Fiscal year reopened successfully" });
            })
            .WithName("ReopenRetainedEarnings")
            .WithSummary("Reopen fiscal year")
            .WithDescription("Reopens a closed fiscal year for retained earnings")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

