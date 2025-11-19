using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.ShiftAssignments.v1;

public static class CreateShiftAssignmentEndpoint
{
    internal static RouteHandlerBuilder MapCreateShiftAssignmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateShiftAssignmentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetShiftAssignmentEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateShiftAssignmentEndpoint))
            .WithSummary("Creates a new shift assignment")
            .WithDescription("Assigns a shift to an employee for a specified date range")
            .Produces<CreateShiftAssignmentResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Attendance))
            .MapToApiVersion(1);
    }
}

