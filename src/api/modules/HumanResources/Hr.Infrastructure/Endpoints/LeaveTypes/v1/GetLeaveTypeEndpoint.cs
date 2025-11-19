using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveTypes.v1;

public static class GetLeaveTypeEndpoint
{
    internal static RouteHandlerBuilder MapGetLeaveTypeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetLeaveTypeRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetLeaveTypeEndpoint))
            .WithSummary("Gets leave type by ID")
            .WithDescription("Retrieves detailed information about a specific leave type including accrual rules and approval requirements")
            .Produces<LeaveTypeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}

