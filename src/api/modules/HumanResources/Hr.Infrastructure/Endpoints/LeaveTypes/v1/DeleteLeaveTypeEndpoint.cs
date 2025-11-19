using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveTypes.v1;

public static class DeleteLeaveTypeEndpoint
{
    internal static RouteHandlerBuilder MapDeleteLeaveTypeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteLeaveTypeCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteLeaveTypeEndpoint))
            .WithSummary("Deletes a leave type")
            .WithDescription("Removes a leave type from the system")
            .Produces<DeleteLeaveTypeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}

