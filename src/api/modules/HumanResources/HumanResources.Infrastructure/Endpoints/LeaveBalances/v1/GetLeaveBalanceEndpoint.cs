using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances.v1;

public static class GetLeaveBalanceEndpoint
{
    internal static RouteHandlerBuilder MapGetLeaveBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetLeaveBalanceRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetLeaveBalanceEndpoint))
            .WithSummary("Gets leave balance by ID")
            .WithDescription("Retrieves detailed information about a specific leave balance including opening, used, and remaining days")
            .Produces<LeaveBalanceResponse>()
            .RequirePermission("Permissions.LeaveBalances.View")
            .MapToApiVersion(1);
    }
}

