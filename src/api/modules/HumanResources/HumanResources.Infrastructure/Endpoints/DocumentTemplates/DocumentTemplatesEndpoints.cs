using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DocumentTemplates.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DocumentTemplates;

public static class DocumentTemplatesEndpoints
{
    internal static IEndpointRouteBuilder MapDocumentTemplatesEndpoints(this IEndpointRouteBuilder app)
    {
        var templatesGroup = app.MapGroup("/document-templates")
            .WithTags("Document Templates")
            .WithDescription("Endpoints for managing document templates");

        templatesGroup.MapCreateDocumentTemplateEndpoint();
        templatesGroup.MapGetDocumentTemplateEndpoint();
        templatesGroup.MapSearchDocumentTemplatesEndpoint();
        templatesGroup.MapUpdateDocumentTemplateEndpoint();
        templatesGroup.MapDeleteDocumentTemplateEndpoint();

        return app;
    }
}

