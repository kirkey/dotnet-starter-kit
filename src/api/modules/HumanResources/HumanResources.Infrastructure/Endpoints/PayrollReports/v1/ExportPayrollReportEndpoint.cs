using System.ComponentModel;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Export.v1;
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
            // TODO: Implement export logic for Excel/PDF/CSV
            return Results.Ok(new { message = "Payroll report export functionality to be implemented" });
        })
        .WithName(nameof(ExportPayrollReportEndpoint))
        .WithSummary("Export payroll report")
        .WithDescription("Exports a payroll report in specified format (Excel/PDF/CSV)")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}


