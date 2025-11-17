using Accounting.Application.Bills.LineItems.Get.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Bills.LineItems.v1;

/// <summary>
/// Endpoint for getting a single bill line item by ID.
/// </summary>
public static class GetBillLineItemEndpoint
{
    internal static RouteHandlerBuilder MapGetBillLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{billId:guid}/line-items/{lineItemId:guid}", async (
                DefaultIdType billId,
                DefaultIdType lineItemId,
                ISender mediator) =>
            {
                var response = await mediator.Send(new GetBillLineItemRequest(lineItemId)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetBillLineItemEndpoint))
            .WithSummary("Get bill line item by ID")
            .WithDescription("Retrieves a specific line item by its identifier.")
            .WithTags("Bill Line Items")
            .Produces<BillLineItemResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
