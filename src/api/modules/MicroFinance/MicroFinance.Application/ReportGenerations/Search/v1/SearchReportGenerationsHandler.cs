using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Search.v1;

/// <summary>
/// Handler for searching report generations.
/// </summary>
public sealed class SearchReportGenerationsHandler(
    [FromKeyedServices("microfinance:reportgenerations")] IReadRepository<ReportGeneration> repository)
    : IRequestHandler<SearchReportGenerationsCommand, PagedList<ReportGenerationResponse>>
{
    public async Task<PagedList<ReportGenerationResponse>> Handle(SearchReportGenerationsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchReportGenerationsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ReportGenerationResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

