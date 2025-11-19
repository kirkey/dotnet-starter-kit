using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Accrue.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances.v1;

public static class AccrueLeaveEndpoint
{
    internal static RouteHandlerBuilder MapAccrueLeaveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/accrue", async (AccrueLeaveCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccrueLeaveEndpoint))
            .WithSummary("Accrues leave to a balance")
            .WithDescription("Adds accrued leave amount to a leave balance based on accrual frequency")
            .Produces<AccrueLeaveResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Accrue, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}
