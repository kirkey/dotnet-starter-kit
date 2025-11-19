using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.ShiftAssignments.v1;

public static class UpdateShiftAssignmentEndpoint
{
    internal static RouteHandlerBuilder MapUpdateShiftAssignmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateShiftAssignmentCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Route ID and request ID do not match.");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateShiftAssignmentEndpoint))
            .WithSummary("Updates a shift assignment")
            .WithDescription("Updates the dates, recurrence, or notes for a shift assignment")
            .Produces<UpdateShiftAssignmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Attendance))
            .MapToApiVersion(1);
    }
}

