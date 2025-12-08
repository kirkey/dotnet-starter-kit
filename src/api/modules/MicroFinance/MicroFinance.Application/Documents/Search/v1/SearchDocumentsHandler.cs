using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Search.v1;

public sealed class SearchDocumentsHandler(
    [FromKeyedServices("microfinance:documents")] IReadRepository<Document> repository)
    : IRequestHandler<SearchDocumentsCommand, PagedList<DocumentSummaryResponse>>
{
    public async Task<PagedList<DocumentSummaryResponse>> Handle(
        SearchDocumentsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchDocumentsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<DocumentSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
