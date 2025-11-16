using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests.v1;

/// <summary>
/// Endpoint for updating a leave request.
/// Primarily used for administrative updates to request status and comments.
/// </summary>
public static class UpdateLeaveRequestEndpoint
{
    internal static RouteHandlerBuilder MapUpdateLeaveRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateLeaveRequestCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateLeaveRequestEndpoint))
            .WithSummary("Updates a leave request")
            .WithDescription("Updates a leave request including status and approver comments. Primarily used for administrative updates.")
            .Produces<UpdateLeaveRequestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}
