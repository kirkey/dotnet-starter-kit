using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments.v1;

public static class GetGeneratedDocumentEndpoint
{
    internal static RouteHandlerBuilder MapGetGeneratedDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetGeneratedDocumentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetGeneratedDocumentEndpoint))
            .WithSummary("Gets generated document by ID")
            .WithDescription("Retrieves generated document details")
            .Produces<GeneratedDocumentResponse>()
            .RequirePermission("Permissions.Documents.View")
            .MapToApiVersion(1);
    }
}

