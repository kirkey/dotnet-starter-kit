using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DocumentTemplates.v1;

public static class SearchDocumentTemplatesEndpoint
{
    internal static RouteHandlerBuilder MapSearchDocumentTemplatesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchDocumentTemplatesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchDocumentTemplatesEndpoint))
            .WithSummary("Searches document templates")
            .WithDescription("Searches document templates with pagination and filters")
            .Produces<PagedList<DocumentTemplateResponse>>()
            .RequirePermission("Permissions.Documents.View")
            .MapToApiVersion(1);
    }
}

