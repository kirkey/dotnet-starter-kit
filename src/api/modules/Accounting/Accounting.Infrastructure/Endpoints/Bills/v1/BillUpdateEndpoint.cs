using Accounting.Application.Bills.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for updating existing bills in the accounting system.
/// </summary>
public static class BillUpdateEndpoint
{
    /// <summary>
    /// Maps the bill update endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapBillUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateBillCommand request, ISender mediator) =>
            {
                var command = request with { BillId = id };
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(nameof(BillUpdateEndpoint))
            .WithSummary("Update an existing bill")
            .WithDescription("Updates an existing bill in the accounts payable system with validation.")
            .Produces<UpdateBillResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

