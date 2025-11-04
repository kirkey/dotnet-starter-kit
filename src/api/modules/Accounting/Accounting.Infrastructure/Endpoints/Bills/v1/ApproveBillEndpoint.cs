using Accounting.Application.Bills.Approve.v1;
using Asp.Versioning;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for approving bills.
/// </summary>
public static class ApproveBillEndpoint
{
    /// <summary>
    /// Maps the bill approval endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapApproveBillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/approve", async (DefaultIdType id, ApproveBillRequest request, ISender mediator) =>
            {
                var command = new ApproveBillCommand(id, request.ApprovedBy);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApproveBillEndpoint))
            .WithSummary("Approve a bill")
            .WithDescription("Approves a bill for payment processing.")
            .Produces<ApproveBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Approve")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

/// <summary>
/// Request to approve a bill.
/// </summary>
public sealed record ApproveBillRequest(string ApprovedBy);
