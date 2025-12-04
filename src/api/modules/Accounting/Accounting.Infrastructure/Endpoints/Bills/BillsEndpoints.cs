using Accounting.Application.Bills.Approve.v1;
using Accounting.Application.Bills.Create.v1;
using Accounting.Application.Bills.Delete.v1;
using Accounting.Application.Bills.Get.v1;
using Accounting.Application.Bills.LineItems.Create.v1;
using Accounting.Application.Bills.LineItems.Delete.v1;
using Accounting.Application.Bills.LineItems.Get.v1;
using Accounting.Application.Bills.LineItems.GetList.v1;
using Accounting.Application.Bills.LineItems.Update.v1;
using Accounting.Application.Bills.MarkAsPaid.v1;
using Accounting.Application.Bills.Post.v1;
using Accounting.Application.Bills.Reject.v1;
using Accounting.Application.Bills.Search.v1;
using Accounting.Application.Bills.Update.v1;
using Accounting.Application.Bills.Void.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Bills;

/// <summary>
/// Endpoint configuration for Bills module.
/// </summary>
public class BillsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Bill endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/bills").WithTags("bills");

        // Create endpoint
        group.MapPost("/", async (BillCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/bills/{response.BillId}", response);
            })
            .WithName("CreateBill")
            .WithSummary("Create a new bill")
            .WithDescription("Creates a new bill in the accounts payable system with comprehensive validation.")
            .Produces<BillCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBillRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBill")
            .WithSummary("Get bill by ID")
            .WithDescription("Retrieves a bill with all line items.")
            .Produces<BillResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, BillUpdateCommand request, ISender mediator) =>
            {
                var command = request with { BillId = id };
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("UpdateBill")
            .WithSummary("Update an existing bill")
            .WithDescription("Updates an existing bill in the accounts payable system with validation.")
            .Produces<UpdateBillResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteBillCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteBill")
            .WithSummary("Delete a bill")
            .WithDescription("Deletes a draft bill. Cannot delete posted or paid bills.")
            .Produces<DeleteBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        // Search endpoint
        group.MapPost("/search", async ([FromBody] SearchBillsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchBills")
            .WithSummary("Search bills")
            .WithDescription("Search and filter bills with pagination.")
            .Produces<PagedList<BillResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Approve endpoint
        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new ApproveBillCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("ApproveBill")
            .WithSummary("Approve a bill")
            .WithDescription("Approves a bill for payment processing. The approver is automatically determined from the current user session.")
            .Produces<ApproveBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting));

        // Reject endpoint
        group.MapPut("/{id:guid}/reject", async (DefaultIdType id, RejectBillRequest request, ISender mediator) =>
            {
                var command = new RejectBillCommand(id, request.RejectedBy, request.Reason);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("RejectBill")
            .WithSummary("Reject a bill")
            .WithDescription("Rejects a bill with a reason.")
            .Produces<RejectBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting));

        // Post endpoint
        group.MapPut("/{id:guid}/post", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new PostBillCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("PostBill")
            .WithSummary("Post a bill to GL")
            .WithDescription("Posts an approved bill to the general ledger.")
            .Produces<PostBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting));

        // Mark as paid endpoint
        group.MapPut("/{id:guid}/mark-paid", async (DefaultIdType id, MarkBillAsPaidRequest request, ISender mediator) =>
            {
                var command = new MarkBillAsPaidCommand(id, request.PaidDate);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("MarkBillAsPaid")
            .WithSummary("Mark bill as paid")
            .WithDescription("Marks a bill as paid with the payment date.")
            .Produces<MarkBillAsPaidResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.MarkAsPaid, FshResources.Accounting));

        // Void endpoint
        group.MapPut("/{id:guid}/void", async (DefaultIdType id, VoidBillRequest request, ISender mediator) =>
            {
                var command = new VoidBillCommand(id, request.Reason);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("VoidBill")
            .WithSummary("Void a bill")
            .WithDescription("Voids a bill with a reason.")
            .Produces<VoidBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Void, FshResources.Accounting));

        // Bill Line Items endpoints

        // Add line item endpoint
        group.MapPost("/{billId:guid}/line-items", async (DefaultIdType billId, AddBillLineItemCommand request, ISender mediator) =>
            {
                if (billId != request.BillId)
                    return Results.BadRequest("Route bill ID does not match command bill ID");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/accounting/bills/{billId}/line-items/{response.LineItemId}", response);
            })
            .WithName("AddBillLineItem")
            .WithSummary("Add a line item to a bill")
            .WithDescription("Adds a new line item to an existing bill and recalculates the total.")
            .WithTags("Bill Line Items")
            .Produces<AddBillLineItemResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Get line items endpoint
        group.MapGet("/{billId:guid}/line-items", async (DefaultIdType billId, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBillLineItemsRequest(billId)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBillLineItems")
            .WithSummary("Get all line items for a bill")
            .WithDescription("Retrieves all line items for a specific bill.")
            .WithTags("Bill Line Items")
            .Produces<List<BillLineItemResponse>>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Get single line item endpoint
        group.MapGet("/{billId:guid}/line-items/{lineItemId:guid}", async (DefaultIdType billId, DefaultIdType lineItemId, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBillLineItemRequest(lineItemId)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBillLineItem")
            .WithSummary("Get bill line item by ID")
            .WithDescription("Retrieves a specific line item by its identifier.")
            .WithTags("Bill Line Items")
            .Produces<BillLineItemResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update line item endpoint
        group.MapPut("/{billId:guid}/line-items/{lineItemId:guid}", async (
            DefaultIdType billId,
            DefaultIdType lineItemId,
            UpdateBillLineItemCommand request,
            ISender mediator) =>
            {
                var command = request with { BillId = billId, LineItemId = lineItemId };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateBillLineItem")
            .WithSummary("Update a bill line item")
            .WithDescription("Updates an existing line item and recalculates the bill total.")
            .WithTags("Bill Line Items")
            .Produces<UpdateBillLineItemResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete line item endpoint
        group.MapDelete("/{billId:guid}/line-items/{lineItemId:guid}", async (
            DefaultIdType billId,
            DefaultIdType lineItemId,
            ISender mediator) =>
            {
                var command = new DeleteBillLineItemCommand(lineItemId, billId);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteBillLineItem")
            .WithSummary("Delete a bill line item")
            .WithDescription("Deletes a line item and recalculates the bill total.")
            .WithTags("Bill Line Items")
            .Produces<DeleteBillLineItemResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));
    }
}

/// <summary>
/// Request to reject a bill.
/// </summary>
public sealed record RejectBillRequest(string RejectedBy, string Reason);

/// <summary>
/// Request to mark a bill as paid.
/// </summary>
public sealed record MarkBillAsPaidRequest(DateTime PaidDate);

/// <summary>
/// Request to void a bill.
/// </summary>
public sealed record VoidBillRequest(string Reason);

