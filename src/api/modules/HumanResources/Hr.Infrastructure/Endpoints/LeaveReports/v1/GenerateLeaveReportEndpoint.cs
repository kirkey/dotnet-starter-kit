namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Generate.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for generating leave reports.
/// </summary>
public static class GenerateLeaveReportEndpoint
{
    /// <summary>
    /// Maps the generate leave report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGenerateLeaveReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/generate", async (GenerateLeaveReportCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(GenerateLeaveReportEndpoint), new { id = response.ReportId }, response);
        })
        .WithName(nameof(GenerateLeaveReportEndpoint))
        .WithSummary("Generate leave report")
        .WithDescription("Generates a leave report based on specified criteria and report type")
        .Produces<GenerateLeaveReportResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Leaves))
        .MapToApiVersion(1);
    }
}

