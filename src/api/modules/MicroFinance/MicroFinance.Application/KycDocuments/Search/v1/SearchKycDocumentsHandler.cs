using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Search.v1;

public sealed class SearchKycDocumentsHandler(
    [FromKeyedServices("microfinance:kycdocuments")] IReadRepository<KycDocument> repository)
    : IRequestHandler<SearchKycDocumentsCommand, PagedList<KycDocumentSummaryResponse>>
{
    public async Task<PagedList<KycDocumentSummaryResponse>> Handle(
        SearchKycDocumentsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchKycDocumentsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<KycDocumentSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
