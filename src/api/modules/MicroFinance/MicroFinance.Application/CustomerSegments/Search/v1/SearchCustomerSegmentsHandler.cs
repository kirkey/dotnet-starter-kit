using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Search.v1;

public sealed class SearchCustomerSegmentsHandler(
    [FromKeyedServices("microfinance:customersegments")] IReadRepository<CustomerSegment> repository)
    : IRequestHandler<SearchCustomerSegmentsCommand, PagedList<CustomerSegmentSummaryResponse>>
{
    public async Task<PagedList<CustomerSegmentSummaryResponse>> Handle(
        SearchCustomerSegmentsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCustomerSegmentsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CustomerSegmentSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
