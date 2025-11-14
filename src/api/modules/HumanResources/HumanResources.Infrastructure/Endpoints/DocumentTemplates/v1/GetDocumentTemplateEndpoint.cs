using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DocumentTemplates.v1;

public static class GetDocumentTemplateEndpoint
{
    internal static RouteHandlerBuilder MapGetDocumentTemplateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetDocumentTemplateRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetDocumentTemplateEndpoint))
            .WithSummary("Gets document template by ID")
            .WithDescription("Retrieves document template details")
            .Produces<DocumentTemplateResponse>()
            .RequirePermission("Permissions.Documents.View")
            .MapToApiVersion(1);
    }
}

