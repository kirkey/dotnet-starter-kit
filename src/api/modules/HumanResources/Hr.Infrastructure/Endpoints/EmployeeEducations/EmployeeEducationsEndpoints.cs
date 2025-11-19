using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeEducations.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeEducations;

/// <summary>
/// Endpoint configuration for EmployeeEducations module.
/// </summary>
public static class EmployeeEducationsEndpoints
{
    /// <summary>
    /// Maps all EmployeeEducations endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapEmployeeEducationsEndpoints(this IEndpointRouteBuilder app)
    {
        var educationsGroup = app.MapGroup("/employee-education")
            .WithTags("Employee Educations")
            .WithDescription("Endpoints for managing employee education records and qualifications");

        // Version 1 endpoints
        educationsGroup.MapCreateEmployeeEducationEndpoint();
        educationsGroup.MapGetEmployeeEducationEndpoint();
        educationsGroup.MapSearchEmployeeEducationsEndpoint();
        educationsGroup.MapUpdateEmployeeEducationEndpoint();
        educationsGroup.MapDeleteEmployeeEducationEndpoint();

        return app;
    }
}

