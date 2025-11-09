using Accounting.Application.Bills.LineItems.Update.v1;

namespace Accounting.Infrastructure.Endpoints.Bills.LineItems.v1;

/// <summary>
/// Endpoint for updating a bill line item.
/// </summary>
public static class UpdateBillLineItemEndpoint
{
    internal static RouteHandlerBuilder MapUpdateBillLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{billId:guid}/line-items/{lineItemId:guid}", async (
                DefaultIdType billId,
                DefaultIdType lineItemId,
                UpdateBillLineItemCommand request,
                ISender mediator) =>
            {
                var command = request with { BillId = billId, LineItemId = lineItemId };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBillLineItemEndpoint))
            .WithSummary("Update a bill line item")
            .WithDescription("Updates an existing line item and recalculates the bill total.")
            .WithTags("Bill Line Items")
            .Produces<UpdateBillLineItemResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Edit")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

