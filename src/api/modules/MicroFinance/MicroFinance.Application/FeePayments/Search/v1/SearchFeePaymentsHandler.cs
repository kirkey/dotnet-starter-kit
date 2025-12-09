using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Search.v1;

public sealed class SearchFeePaymentsHandler(
    [FromKeyedServices("microfinance:feepayments")] IReadRepository<FeePayment> repository)
    : IRequestHandler<SearchFeePaymentsCommand, PagedList<FeePaymentSummaryResponse>>
{
    public async Task<PagedList<FeePaymentSummaryResponse>> Handle(
        SearchFeePaymentsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchFeePaymentsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<FeePaymentSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
