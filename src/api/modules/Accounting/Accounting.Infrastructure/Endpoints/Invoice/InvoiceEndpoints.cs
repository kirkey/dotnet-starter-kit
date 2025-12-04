using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;
using Accounting.Application.Invoices.Create.v1;
using Accounting.Application.Invoices.Update.v1;
using Accounting.Application.Invoices.Delete.v1;
using Accounting.Application.Invoices.Get.v1;
using Accounting.Application.Invoices.Search.v1;
using Accounting.Application.Invoices.Send.v1;
using Accounting.Application.Invoices.MarkPaid.v1;
using Accounting.Application.Invoices.ApplyPayment.v1;
using Accounting.Application.Invoices.Cancel.v1;
using Accounting.Application.Invoices.Void.v1;
using Accounting.Application.Invoices.LineItems.Add.v1;
using Accounting.Application.Invoices.LineItems.Update.v1;
using Accounting.Application.Invoices.LineItems.Delete.v1;
using Accounting.Application.Invoices.LineItems.Get.v1;
using Accounting.Application.Invoices.LineItems.GetList.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice;

/// <summary>
/// Endpoint configuration for Invoice module.
/// </summary>
public class InvoiceEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/invoices").WithTags("invoices");

        // Invoice CRUD endpoints
        group.MapPost("/", async (CreateInvoiceCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Created($"/accounting/invoices/{response.Id}", response);
        })
        .WithName("CreateInvoice")
        .WithSummary("Create a new invoice")
        .WithDescription("Creates a new invoice in the accounts receivable system with comprehensive validation.")
        .Produces<CreateInvoiceResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapPut("/{id:guid}", async (DefaultIdType id, [FromBody] UpdateInvoiceCommand request, ISender mediator) =>
        {
            var command = request with { InvoiceId = id };
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("UpdateInvoice")
        .WithSummary("Update an invoice")
        .WithDescription("Updates an existing invoice. Only Draft invoices can be modified.")
        .Produces<UpdateInvoiceResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new DeleteInvoiceCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("DeleteInvoice")
        .WithSummary("Delete an invoice")
        .WithDescription("Deletes an invoice. Only Draft or Cancelled invoices can be deleted.")
        .Produces<DeleteInvoiceResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetInvoiceRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetInvoice")
        .WithSummary("Get invoice by ID")
        .WithDescription("Retrieves a specific invoice by its unique identifier.")
        .Produces<InvoiceResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapPost("/search", async ([FromBody] SearchInvoicesRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchInvoices")
        .WithSummary("Search invoices")
        .WithDescription("Search and filter invoices with pagination.")
        .Produces<PagedList<InvoiceResponse>>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        // Invoice workflow endpoints
        group.MapPost("/{id:guid}/send", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new SendInvoiceCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SendInvoice")
        .WithSummary("Send an invoice")
        .WithDescription("Transitions invoice status from Draft to Sent.")
        .Produces<SendInvoiceResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapPost("/{id:guid}/mark-paid", async (DefaultIdType id, [FromBody] MarkInvoiceAsPaidCommand command, ISender mediator) =>
        {
            if (id != command.InvoiceId)
            {
                return Results.BadRequest("Invoice ID in URL does not match the request body.");
            }

            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("MarkInvoiceAsPaid")
        .WithSummary("Mark invoice as paid")
        .WithDescription("Marks an invoice as fully paid with payment details.")
        .Produces<MarkInvoiceAsPaidResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapPost("/{id:guid}/apply-payment", async (DefaultIdType id, [FromBody] ApplyInvoicePaymentCommand command, ISender mediator) =>
        {
            if (id != command.InvoiceId)
            {
                return Results.BadRequest("Invoice ID in URL does not match the request body.");
            }

            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("ApplyInvoicePayment")
        .WithSummary("Apply payment to invoice")
        .WithDescription("Applies a partial payment to an invoice. Automatically marks as paid when total payments meet invoice amount.")
        .Produces<ApplyInvoicePaymentResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapPost("/{id:guid}/cancel", async (DefaultIdType id, [FromBody] CancelInvoiceCommand command, ISender mediator) =>
        {
            if (id != command.InvoiceId)
            {
                return Results.BadRequest("Invoice ID in URL does not match the request body.");
            }

            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CancelInvoice")
        .WithSummary("Cancel an invoice")
        .WithDescription("Cancels an unpaid invoice with an optional reason.")
        .Produces<CancelInvoiceResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapPost("/{id:guid}/void", async (DefaultIdType id, [FromBody] VoidInvoiceCommand command, ISender mediator) =>
        {
            if (id != command.InvoiceId)
            {
                return Results.BadRequest("Invoice ID in URL does not match the request body.");
            }

            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("VoidInvoice")
        .WithSummary("Void an invoice")
        .WithDescription("Voids an invoice to reverse accounting impact while maintaining audit trail.")
        .Produces<VoidInvoiceResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        // Invoice line items endpoints
        group.MapPost("/{invoiceId:guid}/line-items", async (DefaultIdType invoiceId, [FromBody] AddInvoiceLineItemCommand command, ISender mediator) =>
        {
            if (invoiceId != command.InvoiceId)
            {
                return Results.BadRequest("Invoice ID in URL does not match the request body.");
            }

            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Created($"/accounting/invoices/{invoiceId}/line-items", response);
        })
        .WithName("AddInvoiceLineItem")
        .WithSummary("Add line item to invoice")
        .WithDescription("Adds a new line item to an invoice and updates the total amount.")
        .Produces<AddInvoiceLineItemResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapPut("/line-items/{lineItemId:guid}", async (DefaultIdType lineItemId, [FromBody] UpdateInvoiceLineItemCommand request, ISender mediator) =>
        {
            var command = request with { LineItemId = lineItemId };
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("UpdateInvoiceLineItem")
        .WithSummary("Update invoice line item")
        .WithDescription("Updates an existing invoice line item and recalculates totals.")
        .Produces<UpdateInvoiceLineItemResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapDelete("/line-items/{lineItemId:guid}", async (DefaultIdType lineItemId, ISender mediator) =>
        {
            var response = await mediator.Send(new DeleteInvoiceLineItemCommand(lineItemId)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("DeleteInvoiceLineItem")
        .WithSummary("Delete invoice line item")
        .WithDescription("Deletes an invoice line item and recalculates invoice totals.")
        .Produces<DeleteInvoiceLineItemResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapGet("/line-items/{lineItemId:guid}", async (DefaultIdType lineItemId, ISender mediator) =>
        {
            var response = await mediator.Send(new GetInvoiceLineItemRequest(lineItemId)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetInvoiceLineItem")
        .WithSummary("Get invoice line item by ID")
        .WithDescription("Retrieves a specific invoice line item by its unique identifier.")
        .Produces<InvoiceLineItemResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));

        group.MapGet("/{invoiceId:guid}/line-items", async (DefaultIdType invoiceId, ISender mediator) =>
        {
            var response = await mediator.Send(new GetInvoiceLineItemsRequest(invoiceId)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetInvoiceLineItems")
        .WithSummary("Get all line items for an invoice")
        .WithDescription("Retrieves all line items associated with a specific invoice.")
        .Produces<List<InvoiceLineItemResponse>>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}
