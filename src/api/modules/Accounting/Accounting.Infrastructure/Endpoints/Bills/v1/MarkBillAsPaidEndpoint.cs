using Accounting.Application.Bills.MarkAsPaid.v1;
using Asp.Versioning;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for marking bills as paid.
/// </summary>
public static class MarkBillAsPaidEndpoint
{
    /// <summary>
    /// Maps the mark bill as paid endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapMarkBillAsPaidEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/mark-paid", async (DefaultIdType id, MarkBillAsPaidRequest request, ISender mediator) =>
            {
                var command = new MarkBillAsPaidCommand(id, request.PaidDate);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkBillAsPaidEndpoint))
            .WithSummary("Mark bill as paid")
            .WithDescription("Marks a bill as paid with the payment date.")
            .Produces<MarkBillAsPaidResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.MarkPaid")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

/// <summary>
/// Request to mark a bill as paid.
/// </summary>
public sealed record MarkBillAsPaidRequest(DateTime PaidDate);

