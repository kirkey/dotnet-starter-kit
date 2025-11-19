using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments;

public static class GeneratedDocumentsEndpoints
{
    internal static IEndpointRouteBuilder MapGeneratedDocumentsEndpoints(this IEndpointRouteBuilder app)
    {
        var docsGroup = app.MapGroup("/generated-documents")
            .WithTags("Generated Documents")
            .WithDescription("Endpoints for managing generated documents");

        docsGroup.MapCreateGeneratedDocumentEndpoint();
        docsGroup.MapGetGeneratedDocumentEndpoint();
        docsGroup.MapSearchGeneratedDocumentsEndpoint();
        docsGroup.MapUpdateGeneratedDocumentEndpoint();
        docsGroup.MapDeleteGeneratedDocumentEndpoint();

        return app;
    }
}

