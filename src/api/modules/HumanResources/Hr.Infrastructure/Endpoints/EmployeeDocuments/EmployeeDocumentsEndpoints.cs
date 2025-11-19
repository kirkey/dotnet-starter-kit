using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments;

public static class EmployeeDocumentsEndpoints
{
    internal static IEndpointRouteBuilder MapEmployeeDocumentsEndpoints(this IEndpointRouteBuilder app)
    {
        var documentsGroup = app.MapGroup("/employee-documents")
            .WithTags("Employee Documents")
            .WithDescription("Endpoints for managing employee documents (contracts, certifications, licenses, etc.)");

        documentsGroup.MapCreateEmployeeDocumentEndpoint();
        documentsGroup.MapGetEmployeeDocumentEndpoint();
        documentsGroup.MapSearchEmployeeDocumentsEndpoint();
        documentsGroup.MapUpdateEmployeeDocumentEndpoint();
        documentsGroup.MapDeleteEmployeeDocumentEndpoint();

        return app;
    }
}

