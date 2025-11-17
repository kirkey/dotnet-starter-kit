namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollReports;

using v1;

/// <summary>
/// Payroll Reports endpoints coordinator.
/// </summary>
public static class PayrollReportsEndpoints
{
    /// <summary>
    /// Maps all payroll report endpoints.
    /// </summary>
    public static void MapPayrollReportsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("payroll-reports").WithTags("Payroll Reports");
        group.MapGeneratePayrollReportEndpoint();
        group.MapGetPayrollReportEndpoint();
        group.MapSearchPayrollReportsEndpoint();
        group.MapDownloadPayrollReportEndpoint();
        group.MapExportPayrollReportEndpoint();
    }
}

