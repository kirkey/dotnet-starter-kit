using Accounting.Application.Bills.Update.v1;
using Asp.Versioning;

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
            .MapPut("/{id:guid}", async (DefaultIdType id, BillUpdateCommand command, ISender mediator) =>
            {
                if (id != command.BillId)
                {
                    return Results.BadRequest("Route ID does not match command ID");
                }

                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(nameof(BillUpdateEndpoint))
            .WithSummary("Update an existing bill")
            .WithDescription("Updates an existing bill in the accounts payable system with validation.")
            .Produces<UpdateBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Edit")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

