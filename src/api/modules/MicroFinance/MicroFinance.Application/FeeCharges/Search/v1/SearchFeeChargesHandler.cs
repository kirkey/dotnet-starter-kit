using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Search.v1;

public sealed class SearchFeeChargesHandler(
    [FromKeyedServices("microfinance:feecharges")] IReadRepository<FeeCharge> repository)
    : IRequestHandler<SearchFeeChargesCommand, PagedList<FeeChargeResponse>>
{
    public async Task<PagedList<FeeChargeResponse>> Handle(SearchFeeChargesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchFeeChargesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<FeeChargeResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
