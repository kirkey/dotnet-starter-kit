namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions;

using v1;

public static class PayrollDeductionEndpoints
{
    internal static void MapPayrollDeductionsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/payroll-deductions")
            .WithTags("PayrollDeductions")
            .WithGroupName("Payroll Management");

        group.MapCreatePayrollDeductionEndpoint();
        group.MapUpdatePayrollDeductionEndpoint();
        group.MapGetPayrollDeductionEndpoint();
        group.MapDeletePayrollDeductionEndpoint();
        group.MapSearchPayrollDeductionsEndpoint();
    }
}
