using FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Export.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Generate.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Search.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollReports;

/// <summary>
/// Endpoint configuration for Payroll Reports module.
/// </summary>
public class PayrollReportsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all Payroll Reports endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/payroll-reports").WithTags("payroll-reports");

        group.MapPost("/generate", async (GeneratePayrollReportCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetPayrollReport", new { id = response.ReportId }, response);
            })
            .WithName("GeneratePayrollReportEndpoint")
            .WithSummary("Generate payroll report")
            .WithDescription("Generates a payroll report based on specified criteria and report type (Summary, Detailed, Departmental, ByEmployee, TaxReport, DeductionReport, BankTransfer)")
            .Produces<GeneratePayrollReportResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetPayrollReportRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPayrollReportEndpoint")
            .WithSummary("Get payroll report")
            .WithDescription("Retrieves a payroll report by ID with all details including totals and averages")
            .Produces<PayrollReportResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchPayrollReportsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPayrollReportsEndpoint")
            .WithSummary("Search payroll reports")
            .WithDescription("Searches and filters payroll reports with pagination support")
            .Produces<PagedList<PayrollReportDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/{id}/export", async (DefaultIdType id, ExportPayrollReportRequest request, ISender mediator) =>
            {
                // TODO: Implement export logic for Excel/PDF/CSV
                return Results.Ok(new { message = "Payroll report export functionality to be implemented" });
            })
            .WithName("ExportPayrollReportEndpoint")
            .WithSummary("Export payroll report")
            .WithDescription("Exports a payroll report in specified format (Excel/PDF/CSV)")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id}/download", async (DefaultIdType id, ISender mediator) =>
            {
                // TODO: Implement report download logic
                // Return file stream with report data
                return Results.Ok(new { message = "Report download functionality to be implemented" });
            })
            .WithName("DownloadPayrollReportEndpoint")
            .WithSummary("Download payroll report")
            .WithDescription("Downloads a payroll report in specified format (PDF/Excel)")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

