using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DocumentTemplates.v1;

public static class UpdateDocumentTemplateEndpoint
{
    internal static RouteHandlerBuilder MapUpdateDocumentTemplateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateDocumentTemplateCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateDocumentTemplateEndpoint))
            .WithSummary("Updates a document template")
            .WithDescription("Updates document template information")
            .Produces<UpdateDocumentTemplateResponse>()
            .RequirePermission("Permissions.Documents.Manage")
            .MapToApiVersion(1);
    }
}

