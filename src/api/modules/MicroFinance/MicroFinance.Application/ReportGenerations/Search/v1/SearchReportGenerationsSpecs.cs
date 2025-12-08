using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Search.v1;

public class SearchReportGenerationsSpecs : EntitiesByPaginationFilterSpec<ReportGeneration, ReportGenerationResponse>
{
    public SearchReportGenerationsSpecs(SearchReportGenerationsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(r => r.CreatedOn, !command.HasOrderBy())
            .Where(r => r.ReportDefinitionId == command.ReportDefinitionId!.Value, command.ReportDefinitionId.HasValue)
            .Where(r => r.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(r => r.Trigger == command.Trigger, !string.IsNullOrWhiteSpace(command.Trigger));
}
