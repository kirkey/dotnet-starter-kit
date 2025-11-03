using Accounting.Application.Bills.LineItems.Get.v1;
using Accounting.Application.Bills.LineItems.GetList.v1;
using Asp.Versioning;

namespace Accounting.Infrastructure.Endpoints.Bills.LineItems.v1;

/// <summary>
/// Endpoint for getting all line items for a bill.
/// </summary>
public static class GetBillLineItemsEndpoint
{
    internal static RouteHandlerBuilder MapGetBillLineItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{billId:guid}/line-items", async (
                DefaultIdType billId,
                ISender mediator) =>
            {
                var response = await mediator.Send(new GetBillLineItemsRequest(billId)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetBillLineItemsEndpoint))
            .WithSummary("Get all line items for a bill")
            .WithDescription("Retrieves all line items for a specific bill.")
            .WithTags("Bill Line Items")
            .Produces<List<BillLineItemResponse>>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Bills.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
