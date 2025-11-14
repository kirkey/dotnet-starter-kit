using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments.v1;

public static class DeleteGeneratedDocumentEndpoint
{
    internal static RouteHandlerBuilder MapDeleteGeneratedDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteGeneratedDocumentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteGeneratedDocumentEndpoint))
            .WithSummary("Deletes a generated document")
            .WithDescription("Deletes a generated document")
            .Produces<DeleteGeneratedDocumentResponse>()
            .RequirePermission("Permissions.Documents.Manage")
            .MapToApiVersion(1);
    }
}

