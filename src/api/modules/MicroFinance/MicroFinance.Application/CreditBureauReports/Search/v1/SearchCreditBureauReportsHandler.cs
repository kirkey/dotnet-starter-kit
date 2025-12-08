using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Search.v1;

public sealed class SearchCreditBureauReportsHandler(
    [FromKeyedServices("microfinance:creditbureaureports")] IReadRepository<CreditBureauReport> repository)
    : IRequestHandler<SearchCreditBureauReportsCommand, PagedList<CreditBureauReportSummaryResponse>>
{
    public async Task<PagedList<CreditBureauReportSummaryResponse>> Handle(
        SearchCreditBureauReportsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCreditBureauReportsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CreditBureauReportSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
