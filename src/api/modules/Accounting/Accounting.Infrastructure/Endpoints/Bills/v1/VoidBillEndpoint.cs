using Accounting.Application.Bills.Void.v1;
using Asp.Versioning;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for voiding bills.
/// </summary>
public static class VoidBillEndpoint
{
    /// <summary>
    /// Maps the bill void endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapVoidBillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/void", async (DefaultIdType id, VoidBillRequest request, ISender mediator) =>
            {
                var command = new VoidBillCommand(id, request.Reason);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(VoidBillEndpoint))
            .WithSummary("Void a bill")
            .WithDescription("Voids a bill with a reason.")
            .Produces<VoidBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Bills.Void")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

/// <summary>
/// Request to void a bill.
/// </summary>
public sealed record VoidBillRequest(string Reason);
