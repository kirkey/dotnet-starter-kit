using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls;

/// <summary>
/// Endpoint routes for managing payroll periods.
/// Supports the complete payroll workflow including creation, processing, GL posting, and payment marking.
/// </summary>
public static class PayrollsEndpoints
{
    internal static IEndpointRouteBuilder MapPayrollsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/payrolls")
            .WithTags("Payrolls")
            .WithDescription("Endpoints for managing payroll periods with support for processing, GL posting, and payment workflows per Philippines accounting standards");

        group.MapCreatePayrollEndpoint();
        group.MapGetPayrollEndpoint();
        group.MapUpdatePayrollEndpoint();
        group.MapDeletePayrollEndpoint();
        group.MapSearchPayrollsEndpoint();
        group.MapProcessPayrollEndpoint();
        group.MapCompletePayrollProcessingEndpoint();
        group.MapPostPayrollEndpoint();
        group.MapMarkPayrollAsPaidEndpoint();

        return app;
    }
}

