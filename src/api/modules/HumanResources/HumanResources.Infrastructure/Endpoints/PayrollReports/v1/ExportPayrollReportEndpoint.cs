using System.ComponentModel;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollReports.v1;

using Shared.Authorization;

/// <summary>
/// Endpoint for exporting payroll reports.
/// </summary>
public static class ExportPayrollReportEndpoint
{
    /// <summary>
    /// Maps the export payroll report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapExportPayrollReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/{id}/export", async (DefaultIdType id, ExportPayrollReportRequest request, ISender mediator) =>
        {
            // TODO: Implement report export logic
            // Support formats: CSV, Excel, PDF, JSON
            return Results.Ok(new { message = "Report export functionality to be implemented" });
        })
        .WithName(nameof(ExportPayrollReportEndpoint))
        .WithSummary("Export payroll report")
        .WithDescription("Exports a payroll report in specified format")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}

/// <summary>
/// Request to export a payroll report.
/// </summary>
public record ExportPayrollReportRequest(
    [property: DefaultValue("Excel")] string Format = "Excel", // Excel, CSV, PDF, JSON
    [property: DefaultValue(null)] bool? IncludeDetails = null);

