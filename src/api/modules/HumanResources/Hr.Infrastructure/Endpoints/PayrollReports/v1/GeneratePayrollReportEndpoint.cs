namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Generate.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for generating payroll reports.
/// </summary>
public static class GeneratePayrollReportEndpoint
{
    /// <summary>
    /// Maps the generate payroll report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGeneratePayrollReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/generate", async (GeneratePayrollReportCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(GeneratePayrollReportEndpoint), new { id = response.ReportId }, response);
        })
        .WithName(nameof(GeneratePayrollReportEndpoint))
        .WithSummary("Generate payroll report")
        .WithDescription("Generates a payroll report based on specified criteria and report type (Summary, Detailed, Departmental, ByEmployee, TaxReport, DeductionReport, BankTransfer)")
        .Produces<GeneratePayrollReportResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}

