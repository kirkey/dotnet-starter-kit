using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.ShiftAssignments.v1;

public static class DeleteShiftAssignmentEndpoint
{
    internal static RouteHandlerBuilder MapDeleteShiftAssignmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteShiftAssignmentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteShiftAssignmentEndpoint))
            .WithSummary("Deletes a shift assignment")
            .WithDescription("Removes a shift assignment from the system")
            .Produces<DeleteShiftAssignmentResponse>()
            .RequirePermission("Permissions.ShiftAssignments.Delete")
            .MapToApiVersion(1);
    }
}

