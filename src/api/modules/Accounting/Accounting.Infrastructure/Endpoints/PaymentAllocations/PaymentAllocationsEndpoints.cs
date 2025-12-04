using Accounting.Application.PaymentAllocations.Commands;
using Accounting.Application.PaymentAllocations.Queries;
using Accounting.Application.PaymentAllocations.Responses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PaymentAllocations;

/// <summary>
/// Endpoint configuration for Payment Allocations module.
/// Provides comprehensive REST API endpoints for managing payment allocations.
/// </summary>
public class PaymentAllocationsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Payment Allocations endpoints to the route builder.
    /// Includes Create, Read, Update, Delete, and Search operations for payment allocations.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/payment-allocations").WithTags("payment-allocations");

        // Create endpoint
        group.MapPost("/", async (CreatePaymentAllocationCommand command, ISender mediator) =>
            {
                var id = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/payment-allocations/{id}", new { Id = id });
            })
            .WithName("CreatePaymentAllocation")
            .WithSummary("Create a new payment allocation")
            .WithDescription("Allocates a payment amount to a specific invoice")
            .Produces<object>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPaymentAllocationByIdQuery { Id = id }).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPaymentAllocation")
            .WithSummary("Gets a payment allocation by id")
            .Produces<PaymentAllocationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePaymentAllocationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var allocationId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = allocationId });
            })
            .WithName("UpdatePaymentAllocation")
            .WithSummary("Update a payment allocation")
            .WithDescription("Updates the amount and/or notes of a payment allocation")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeletePaymentAllocationCommand(id)).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("DeletePaymentAllocation")
            .WithSummary("Deletes a payment allocation")
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async ([FromBody] SearchPaymentAllocationsQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPaymentAllocations")
            .WithSummary("Searches payment allocations")
            .Produces<List<PaymentAllocationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
