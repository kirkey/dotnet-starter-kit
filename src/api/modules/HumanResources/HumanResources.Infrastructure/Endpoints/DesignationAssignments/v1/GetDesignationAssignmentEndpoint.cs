using FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments.v1;

/// <summary>
/// Endpoint for getting a designation assignment by ID.
/// </summary>
public static class GetDesignationAssignmentEndpoint
{
    internal static RouteHandlerBuilder MapGetDesignationAssignmentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator
                    .Send(new GetDesignationAssignmentRequest(id))
                    .ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetDesignationAssignmentEndpoint))
            .WithSummary("Gets designation assignment by ID")
            .WithDescription("Retrieves designation assignment details including tenure and status")
            .Produces<DesignationAssignmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);
    }
}
