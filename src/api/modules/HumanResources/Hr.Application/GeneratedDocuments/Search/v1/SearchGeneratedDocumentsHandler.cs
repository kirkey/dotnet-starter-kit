using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Search.v1;

public sealed class SearchGeneratedDocumentsHandler(
    [FromKeyedServices("hr:generateddocuments")] IReadRepository<GeneratedDocument> repository)
    : IRequestHandler<SearchGeneratedDocumentsRequest, PagedList<GeneratedDocumentResponse>>
{
    public async Task<PagedList<GeneratedDocumentResponse>> Handle(
        SearchGeneratedDocumentsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchGeneratedDocumentsSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<GeneratedDocumentResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

