namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Get.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for retrieving a leave report by ID.
/// </summary>
public static class GetLeaveReportEndpoint
{
    /// <summary>
    /// Maps the get leave report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGetLeaveReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetLeaveReportRequest(id);
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetLeaveReportEndpoint))
        .WithSummary("Get leave report")
        .WithDescription("Retrieves a leave report by ID with all details")
        .Produces<LeaveReportResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
        .MapToApiVersion(1);
    }
}

