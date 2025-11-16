using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.ShiftAssignments.v1;

public static class GetShiftAssignmentEndpoint
{
    internal static RouteHandlerBuilder MapGetShiftAssignmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetShiftAssignmentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetShiftAssignmentEndpoint))
            .WithSummary("Gets shift assignment details")
            .WithDescription("Retrieves detailed information about a specific shift assignment")
            .Produces<ShiftAssignmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
            .MapToApiVersion(1);
    }
}

