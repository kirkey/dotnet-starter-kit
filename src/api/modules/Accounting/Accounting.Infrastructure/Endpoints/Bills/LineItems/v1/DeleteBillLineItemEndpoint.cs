using Accounting.Application.Bills.LineItems.Delete.v1;
using Asp.Versioning;

namespace Accounting.Infrastructure.Endpoints.Bills.LineItems.v1;

/// <summary>
/// Endpoint for deleting a bill line item.
/// </summary>
public static class DeleteBillLineItemEndpoint
{
    internal static RouteHandlerBuilder MapDeleteBillLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{billId:guid}/line-items/{lineItemId:guid}", async (
                DefaultIdType billId,
                DefaultIdType lineItemId,
                ISender mediator) =>
            {
                var command = new DeleteBillLineItemCommand(lineItemId, billId);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteBillLineItemEndpoint))
            .WithSummary("Delete a bill line item")
            .WithDescription("Deletes a line item and recalculates the bill total.")
            .WithTags("Bill Line Items")
            .Produces<DeleteBillLineItemResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

