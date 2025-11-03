using Accounting.Application.Bills.Get.v1;
using Asp.Versioning;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for getting a bill by ID.
/// </summary>
public static class GetBillEndpoint
{
    internal static RouteHandlerBuilder MapGetBillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBillRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetBillEndpoint))
            .WithSummary("Get bill by ID")
            .WithDescription("Retrieves a bill with all line items.")
            .Produces<BillResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Bills.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
