using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments.v1;

public static class UpdateGeneratedDocumentEndpoint
{
    internal static RouteHandlerBuilder MapUpdateGeneratedDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateGeneratedDocumentCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateGeneratedDocumentEndpoint))
            .WithSummary("Updates a generated document")
            .WithDescription("Updates generated document status and information")
            .Produces<UpdateGeneratedDocumentResponse>()
            .RequirePermission("Permissions.Documents.Manage")
            .MapToApiVersion(1);
    }
}

