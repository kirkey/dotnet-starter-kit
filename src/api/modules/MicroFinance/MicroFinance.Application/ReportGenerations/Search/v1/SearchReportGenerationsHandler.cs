using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Specifications;
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

        var spec = new EntitiesByPaginationFilterSpec<ReportGeneration, ReportGenerationResponse>(
            new PaginationFilter
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Keyword = request.Keyword,
                OrderBy = request.OrderBy
            });

        return await repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken)
            .ConfigureAwait(false);
    }
}

