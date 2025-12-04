using Accounting.Application.Payments.Commands;
using Accounting.Application.Payments.Create.v1;
using Accounting.Application.Payments.Delete.v1;
using Accounting.Application.Payments.Get.v1;
using Accounting.Application.Payments.Refund;
using Accounting.Application.Payments.Search.v1;
using Accounting.Application.Payments.Update.v1;
using Accounting.Application.Payments.Void;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Payments;

/// <summary>
/// Endpoint configuration for Payments module.
/// Provides comprehensive REST API endpoints for managing payments.
/// </summary>
public class PaymentsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Payments endpoints to the route builder.
    /// Includes Create, Read, Update, Delete, Search, and workflow operations for payments.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/payments").WithTags("payments");

        // Create endpoint
        group.MapPost("/", async (PaymentCreateCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/accounting/payments/{response.Id}", response);
            })
            .WithName("CreatePayment")
            .WithSummary("Create a new payment")
            .WithDescription("Creates a new payment record for customer/member payments")
            .Produces<PaymentCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get endpoint
        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new PaymentGetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPayment")
            .WithSummary("Get payment by ID")
            .WithDescription("Retrieves a payment by its unique identifier")
            .Produces<PaymentGetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update endpoint
        group.MapPut("/{id}", async (DefaultIdType id, PaymentUpdateCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdatePayment")
            .WithSummary("Update a payment")
            .WithDescription("Updates payment details (reference, deposit account, description, notes)")
            .Produces<PaymentUpdateResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete endpoint
        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new PaymentDeleteCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeletePayment")
            .WithSummary("Delete a payment")
            .WithDescription("Deletes a payment. Cannot delete payments with allocations.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        // Search endpoint
        group.MapPost("/search", async (PaymentSearchRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPayments")
            .WithSummary("Search payments")
            .WithDescription("Searches payments with filtering and pagination")
            .Produces<PagedList<PaymentSearchResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Allocate payment endpoint
        group.MapPost("/allocate", async (AllocatePaymentCommand request, ISender mediator) =>
            {
                await mediator.Send(request).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("AllocatePayment")
            .WithSummary("Allocate a payment")
            .WithDescription("Allocate a payment to invoices")
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Refund payment endpoint
        group.MapPost("/{id}/refund", async (DefaultIdType id, RefundPaymentCommand request, ISender mediator) =>
            {
                if (id != request.PaymentId)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var paymentId = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(new { PaymentId = paymentId, Message = "Payment refunded successfully" });
            })
            .WithName("RefundPayment")
            .WithSummary("Refund a payment")
            .WithDescription("Issues a refund for a payment or partial payment amount")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Void payment endpoint
        group.MapPost("/{id}/void", async (DefaultIdType id, VoidPaymentCommand request, ISender mediator) =>
            {
                if (id != request.PaymentId)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var paymentId = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(new { PaymentId = paymentId, Message = "Payment voided successfully" });
            })
            .WithName("VoidPayment")
            .WithSummary("Void a payment")
            .WithDescription("Voids a payment and reverses all allocations")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));
    }
}
