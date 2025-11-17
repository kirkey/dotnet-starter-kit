using Accounting.Application.Bills.LineItems.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Bills.LineItems.v1;

/// <summary>
/// Endpoint for adding a line item to a bill.
/// </summary>
public static class AddBillLineItemEndpoint
{
    internal static RouteHandlerBuilder MapAddBillLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{billId:guid}/line-items", async (DefaultIdType billId, AddBillLineItemCommand request, ISender mediator) =>
            {
                // Ensure billId from route matches command
                if (billId != request.BillId)
                    return Results.BadRequest("Route bill ID does not match command bill ID");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/accounting/bills/{billId}/line-items/{response.LineItemId}", response);
            })
            .WithName(nameof(AddBillLineItemEndpoint))
            .WithSummary("Add a line item to a bill")
            .WithDescription("Adds a new line item to an existing bill and recalculates the total.")
            .WithTags("Bill Line Items")
            .Produces<AddBillLineItemResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
