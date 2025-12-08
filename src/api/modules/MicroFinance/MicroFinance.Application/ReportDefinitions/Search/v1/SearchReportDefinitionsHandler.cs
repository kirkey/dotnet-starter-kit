// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/ReportDefinitions/Search/v1/SearchReportDefinitionsHandler.cs
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Search.v1;

/// <summary>
/// Handler for searching report definitions.
/// </summary>
public sealed class SearchReportDefinitionsHandler(
    [FromKeyedServices("microfinance:reportdefinitions")] IReadRepository<ReportDefinition> repository)
    : IRequestHandler<SearchReportDefinitionsCommand, PagedList<ReportDefinitionResponse>>
{
    public async Task<PagedList<ReportDefinitionResponse>> Handle(SearchReportDefinitionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchReportDefinitionsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ReportDefinitionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

