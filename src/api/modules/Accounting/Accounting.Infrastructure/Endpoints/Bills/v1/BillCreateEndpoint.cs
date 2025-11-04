using Accounting.Application.Bills.Create.v1;
using Asp.Versioning;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for creating new bills in the accounting system.
/// </summary>
public static class BillCreateEndpoint
{
    /// <summary>
    /// Maps the bill creation endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapBillCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (BillCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/bills/{response.BillId}", response);
            })
            .WithName(nameof(BillCreateEndpoint))
            .WithSummary("Create a new bill")
            .WithDescription("Creates a new bill in the accounts payable system with comprehensive validation.")
            .Produces<BillCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

