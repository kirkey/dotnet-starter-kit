using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts;

/// <summary>
/// Endpoint configuration for EmployeeContacts module.
/// </summary>
public static class EmployeeContactsEndpoints
{
    /// <summary>
    /// Maps all EmployeeContacts endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapEmployeeContactsEndpoints(this IEndpointRouteBuilder app)
    {
        var contactsGroup = app.MapGroup("/employee-contacts")
            .WithTags("Employee Contacts")
            .WithDescription("Endpoints for managing employee contacts (emergency, references, family)");

        // Version 1 endpoints
        contactsGroup.MapCreateEmployeeContactEndpoint();
        contactsGroup.MapGetEmployeeContactEndpoint();
        contactsGroup.MapSearchEmployeeContactsEndpoint();
        contactsGroup.MapUpdateEmployeeContactEndpoint();
        contactsGroup.MapDeleteEmployeeContactEndpoint();

        return app;
    }
}

