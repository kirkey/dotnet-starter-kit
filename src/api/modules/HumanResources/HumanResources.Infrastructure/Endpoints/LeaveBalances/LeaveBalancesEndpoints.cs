using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances;

public static class LeaveBalancesEndpoints
{
    internal static IEndpointRouteBuilder MapLeaveBalancesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/leave-balances")
            .WithTags("Leave Balances")
            .WithDescription("Endpoints for managing employee leave balances and accruals");

        group.MapCreateLeaveBalanceEndpoint();
        group.MapGetLeaveBalanceEndpoint();
        group.MapUpdateLeaveBalanceEndpoint();
        group.MapDeleteLeaveBalanceEndpoint();
        group.MapSearchLeaveBalancesEndpoint();
        group.MapAccrueLeaveEndpoint();

        return app;
    }
}

