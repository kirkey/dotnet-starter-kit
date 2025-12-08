using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Search.v1;

public sealed class SearchCustomerSurveysHandler(
    [FromKeyedServices("microfinance:customersurveys")] IReadRepository<CustomerSurvey> repository)
    : IRequestHandler<SearchCustomerSurveysCommand, PagedList<CustomerSurveySummaryResponse>>
{
    public async Task<PagedList<CustomerSurveySummaryResponse>> Handle(
        SearchCustomerSurveysCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCustomerSurveysSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CustomerSurveySummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
