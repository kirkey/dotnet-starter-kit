using Accounting.Application.Bills.Reject.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for rejecting bills.
/// </summary>
public static class RejectBillEndpoint
{
    /// <summary>
    /// Maps the bill rejection endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapRejectBillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}/reject", async (DefaultIdType id, RejectBillRequest request, ISender mediator) =>
            {
                var command = new RejectBillCommand(id, request.RejectedBy, request.Reason);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RejectBillEndpoint))
            .WithSummary("Reject a bill")
            .WithDescription("Rejects a bill with a reason.")
            .Produces<RejectBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

/// <summary>
/// Request to reject a bill.
/// </summary>
public sealed record RejectBillRequest(string RejectedBy, string Reason);

