using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollLines.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollLines;

/// <summary>
/// Endpoint routes for managing payroll lines (employee pay records).
/// Each payroll line represents one employee's pay calculation for a payroll period.
/// </summary>
public static class PayrollLinesEndpoints
{
    internal static IEndpointRouteBuilder MapPayrollLinesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/payroll-lines")
            .WithTags("Payroll Lines")
            .WithDescription("Endpoints for managing individual employee pay records within payroll periods");

        group.MapCreatePayrollLineEndpoint();
        group.MapGetPayrollLineEndpoint();
        group.MapUpdatePayrollLineEndpoint();
        group.MapDeletePayrollLineEndpoint();
        group.MapSearchPayrollLinesEndpoint();

        return app;
    }
}

