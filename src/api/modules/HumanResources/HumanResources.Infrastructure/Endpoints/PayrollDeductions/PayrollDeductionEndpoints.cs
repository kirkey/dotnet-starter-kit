namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions;

public static class PayrollDeductionEndpoints
{
    internal static void MapPayrollDeductionsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/payroll-deductions")
            .WithTags("PayrollDeductions")
            .WithGroupName("Payroll Management");

        CreatePayrollDeductionEndpoint.MapCreatePayrollDeductionEndpoint(group);
        UpdatePayrollDeductionEndpoint.MapUpdatePayrollDeductionEndpoint(group);
        GetPayrollDeductionEndpoint.MapGetPayrollDeductionEndpoint(group);
        DeletePayrollDeductionEndpoint.MapDeletePayrollDeductionEndpoint(group);
    }
}
