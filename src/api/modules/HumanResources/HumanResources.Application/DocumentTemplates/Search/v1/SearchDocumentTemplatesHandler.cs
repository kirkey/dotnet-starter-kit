using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Search.v1;

public sealed class SearchDocumentTemplatesHandler(
    [FromKeyedServices("hr:documenttemplates")] IReadRepository<DocumentTemplate> repository)
    : IRequestHandler<SearchDocumentTemplatesRequest, PagedList<DocumentTemplateResponse>>
{
    public async Task<PagedList<DocumentTemplateResponse>> Handle(
        SearchDocumentTemplatesRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchDocumentTemplatesSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<DocumentTemplateResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

