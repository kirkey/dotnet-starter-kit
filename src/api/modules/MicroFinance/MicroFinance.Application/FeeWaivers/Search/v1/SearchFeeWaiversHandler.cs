using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Search.v1;

public sealed class SearchFeeWaiversHandler(
    [FromKeyedServices("microfinance:feewaivers")] IReadRepository<FeeWaiver> repository)
    : IRequestHandler<SearchFeeWaiversCommand, PagedList<FeeWaiverSummaryResponse>>
{
    public async Task<PagedList<FeeWaiverSummaryResponse>> Handle(
        SearchFeeWaiversCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchFeeWaiversSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<FeeWaiverSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
