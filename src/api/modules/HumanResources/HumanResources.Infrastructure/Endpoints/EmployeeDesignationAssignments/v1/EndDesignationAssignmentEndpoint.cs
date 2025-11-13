using FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.End.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments.v1;

/// <summary>
/// Request body for ending a designation assignment.
/// </summary>
public sealed record EndDesignationRequest
{
    /// <summary>
    /// End date for the assignment.
    /// </summary>
    public DateTime EndDate { get; init; }

    /// <summary>
    /// Reason for ending the assignment.
    /// </summary>
    public string? Reason { get; init; }
}

/// <summary>
/// Endpoint for ending a designation assignment.
/// </summary>
public static class EndDesignationAssignmentEndpoint
{
    internal static RouteHandlerBuilder MapEndDesignationAssignmentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/end", async (DefaultIdType id, EndDesignationRequest request, ISender mediator) =>
            {
                var response = await mediator
                    .Send(new EndDesignationAssignmentCommand(id, request.EndDate, request.Reason))
                    .ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(EndDesignationAssignmentEndpoint))
            .WithSummary("Ends a designation assignment")
            .WithDescription("Ends an active designation assignment on a specified date")
            .Produces<EndDesignationAssignmentResponse>()
            .RequirePermission("Permissions.EmployeeDesignations.Manage")
            .MapToApiVersion(1);
    }
}

