namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeePayComponents;

public static class EmployeePayComponentEndpoints
{
    internal static void MapEmployeePayComponentsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/employee-paycomponents")
            .WithTags("EmployeePayComponents")
            .WithGroupName("Payroll Management");

        CreateEmployeePayComponentEndpoint.MapCreateEmployeePayComponentEndpoint(group);
        UpdateEmployeePayComponentEndpoint.MapUpdateEmployeePayComponentEndpoint(group);
        GetEmployeePayComponentEndpoint.MapGetEmployeePayComponentEndpoint(group);
        DeleteEmployeePayComponentEndpoint.MapDeleteEmployeePayComponentEndpoint(group);
    }
}

