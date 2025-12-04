using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Search.v1;

public sealed class SearchFixedDepositsHandler(
    [FromKeyedServices("microfinance:fixeddeposits")] IReadRepository<FixedDeposit> repository)
    : IRequestHandler<SearchFixedDepositsCommand, PagedList<FixedDepositResponse>>
{
    public async Task<PagedList<FixedDepositResponse>> Handle(SearchFixedDepositsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchFixedDepositsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<FixedDepositResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
