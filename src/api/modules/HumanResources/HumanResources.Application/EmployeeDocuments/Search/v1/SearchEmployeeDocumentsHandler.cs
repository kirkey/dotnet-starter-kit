using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Search.v1;

public sealed class SearchEmployeeDocumentsHandler(
    [FromKeyedServices("hr:documents")] IReadRepository<EmployeeDocument> repository)
    : IRequestHandler<SearchEmployeeDocumentsRequest, PagedList<EmployeeDocumentResponse>>
{
    public async Task<PagedList<EmployeeDocumentResponse>> Handle(
        SearchEmployeeDocumentsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchEmployeeDocumentsSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<EmployeeDocumentResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

