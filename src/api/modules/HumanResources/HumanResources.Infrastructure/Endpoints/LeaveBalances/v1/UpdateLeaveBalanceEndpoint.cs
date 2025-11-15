using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances.v1;

public static class UpdateLeaveBalanceEndpoint
{
    internal static RouteHandlerBuilder MapUpdateLeaveBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateLeaveBalanceCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Route ID and request ID do not match.");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateLeaveBalanceEndpoint))
            .WithSummary("Updates a leave balance")
            .WithDescription("Updates leave balance opening balance or other details")
            .Produces<UpdateLeaveBalanceResponse>()
            .RequirePermission("Permissions.LeaveBalances.Update")
            .MapToApiVersion(1);
    }
}

