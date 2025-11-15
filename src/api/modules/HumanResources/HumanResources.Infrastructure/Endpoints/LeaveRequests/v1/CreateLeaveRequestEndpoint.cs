using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests.v1;

/// <summary>
/// Endpoint for creating a new leave request.
/// The leave request is created in Draft status and must be submitted for approval.
/// </summary>
public static class CreateLeaveRequestEndpoint
{
    internal static RouteHandlerBuilder MapCreateLeaveRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateLeaveRequestCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetLeaveRequestEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateLeaveRequestEndpoint))
            .WithSummary("Creates a new leave request")
            .WithDescription("Creates a new leave request for an employee. The request is created in Draft status and must be submitted for approval. Validates employee leave balance and leave type eligibility per Philippines Labor Code.")
            .Produces<CreateLeaveRequestResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.LeaveRequests.Create")
            .MapToApiVersion(1);
    }
}

