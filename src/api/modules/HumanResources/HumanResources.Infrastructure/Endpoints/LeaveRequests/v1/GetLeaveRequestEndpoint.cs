using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests.v1;

/// <summary>
/// Endpoint for retrieving a specific leave request by ID.
/// </summary>
public static class GetLeaveRequestEndpoint
{
    internal static RouteHandlerBuilder MapGetLeaveRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetLeaveRequestRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetLeaveRequestEndpoint))
            .WithSummary("Gets a leave request by ID")
            .WithDescription("Retrieves detailed information about a specific leave request including status, dates, approval details, and attachments")
            .Produces<LeaveRequestResponse>(StatusCodes.Status200OK)
            .RequirePermission("Permissions.LeaveRequests.View")
            .MapToApiVersion(1);
    }
}

