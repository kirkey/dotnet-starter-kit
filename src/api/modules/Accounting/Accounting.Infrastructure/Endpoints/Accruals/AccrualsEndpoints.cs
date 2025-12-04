using Accounting.Application.Accruals.Approve;
using Accounting.Application.Accruals.Create;
using Accounting.Application.Accruals.Delete;
using Accounting.Application.Accruals.Queries;
using Accounting.Application.Accruals.Reject;
using Accounting.Application.Accruals.Responses;
using Accounting.Application.Accruals.Reverse;
using Accounting.Application.Accruals.Search;
using Accounting.Application.Accruals.Update;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals;

/// <summary>
/// Endpoint configuration for Accruals module.
/// </summary>
public class AccrualsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Accruals endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/accruals").WithTags("accruals");

        group.MapPost("/", async (CreateAccrualCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/accruals/{response.Id}", response);
            })
            .WithName("CreateAccrual")
            .WithSummary("Create an accrual")
            .WithDescription("Creates a new accrual entry")
            .Produces<CreateAccrualResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAccrualByIdQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetAccrual")
            .WithSummary("Get an accrual by ID")
            .WithDescription("Gets the details of an accrual by its ID")
            .Produces<AccrualResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateAccrualCommand request, ISender mediator) =>
            {
                request.Id = id;
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateAccrual")
            .WithSummary("Update an accrual")
            .WithDescription("Updates an accrual's mutable fields")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteAccrualCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteAccrual")
            .WithSummary("Delete accrual by id")
            .WithDescription("Deletes an accrual entry by its identifier")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        group.MapPut("/{id:guid}/reverse", async (DefaultIdType id, ReverseAccrualCommand command, ISender mediator) =>
            {
                command.Id = id;
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("ReverseAccrual")
            .WithSummary("Reverse an accrual")
            .WithDescription("Reverses an accrual entry by ID")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Void, FshResources.Accounting));

        group.MapPost("/search", async (SearchAccrualsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchAccruals")
            .WithSummary("Search accruals")
            .WithDescription("Search accrual entries with filters and pagination")
            .Produces<PagedList<AccrualResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveAccrualCommand command, ISender mediator) =>
            {
                if (id != command.AccrualId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(result);
            })
            .WithName("ApproveAccrual")
            .WithSummary("Approve accrual")
            .WithDescription("Approves accrual entry")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting));

        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectAccrualCommand command, ISender mediator) =>
            {
                if (id != command.AccrualId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(result);
            })
            .WithName("RejectAccrual")
            .WithSummary("Reject accrual")
            .WithDescription("Rejects accrual entry")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting));
    }
}
