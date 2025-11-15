using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances.v1;

public static class DeleteLeaveBalanceEndpoint
{
    internal static RouteHandlerBuilder MapDeleteLeaveBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteLeaveBalanceCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteLeaveBalanceEndpoint))
            .WithSummary("Deletes a leave balance")
            .WithDescription("Removes a leave balance from the system")
            .Produces<DeleteLeaveBalanceResponse>()
            .RequirePermission("Permissions.LeaveBalances.Delete")
            .MapToApiVersion(1);
    }
}

