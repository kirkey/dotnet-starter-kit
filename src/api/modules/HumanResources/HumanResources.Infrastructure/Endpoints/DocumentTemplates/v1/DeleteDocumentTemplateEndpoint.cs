using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DocumentTemplates.v1;

public static class DeleteDocumentTemplateEndpoint
{
    internal static RouteHandlerBuilder MapDeleteDocumentTemplateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteDocumentTemplateCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteDocumentTemplateEndpoint))
            .WithSummary("Deletes a document template")
            .WithDescription("Deletes a document template")
            .Produces<DeleteDocumentTemplateResponse>()
            .RequirePermission("Permissions.Documents.Manage")
            .MapToApiVersion(1);
    }
}

