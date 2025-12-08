using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Search.v1;

public sealed class SearchCreditBureauInquiriesHandler(
    [FromKeyedServices("microfinance:creditbureauinquiries")] IReadRepository<CreditBureauInquiry> repository)
    : IRequestHandler<SearchCreditBureauInquiriesCommand, PagedList<CreditBureauInquirySummaryResponse>>
{
    public async Task<PagedList<CreditBureauInquirySummaryResponse>> Handle(
        SearchCreditBureauInquiriesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCreditBureauInquiriesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CreditBureauInquirySummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
