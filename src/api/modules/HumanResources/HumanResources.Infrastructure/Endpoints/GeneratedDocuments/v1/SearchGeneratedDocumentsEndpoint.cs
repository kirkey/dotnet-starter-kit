using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments.v1;

public static class SearchGeneratedDocumentsEndpoint
{
    internal static RouteHandlerBuilder MapSearchGeneratedDocumentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchGeneratedDocumentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchGeneratedDocumentsEndpoint))
            .WithSummary("Searches generated documents")
            .WithDescription("Searches generated documents with pagination and filters")
            .Produces<PagedList<GeneratedDocumentResponse>>()
            .RequirePermission("Permissions.Documents.View")
            .MapToApiVersion(1);
    }
}

