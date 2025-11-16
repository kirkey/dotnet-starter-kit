using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveTypes.v1;

public static class UpdateLeaveTypeEndpoint
{
    internal static RouteHandlerBuilder MapUpdateLeaveTypeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateLeaveTypeCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Route ID and request ID do not match.");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateLeaveTypeEndpoint))
            .WithSummary("Updates a leave type")
            .WithDescription("Updates leave type information including accrual allowance, frequency, and approval requirements")
            .Produces<UpdateLeaveTypeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}

