namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.AttendanceReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Generate.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for generating attendance reports.
/// </summary>
public static class GenerateAttendanceReportEndpoint
{
    /// <summary>
    /// Maps the generate attendance report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGenerateAttendanceReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/generate", async (GenerateAttendanceReportCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(GenerateAttendanceReportEndpoint), new { id = response.ReportId }, response);
        })
        .WithName(nameof(GenerateAttendanceReportEndpoint))
        .WithSummary("Generate attendance report")
        .WithDescription("Generates an attendance report based on specified criteria and report type")
        .Produces<GenerateAttendanceReportResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Attendance))
        .MapToApiVersion(1);
    }
}

