using Accounting.Application.Bills.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for deleting bills.
/// </summary>
public static class DeleteBillEndpoint
{
    internal static RouteHandlerBuilder MapDeleteBillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteBillCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteBillEndpoint))
            .WithSummary("Delete a bill")
            .WithDescription("Deletes a draft bill. Cannot delete posted or paid bills.")
            .Produces<DeleteBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
