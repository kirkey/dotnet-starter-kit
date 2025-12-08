// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/ReportDefinitions/Search/v1/SearchReportDefinitionsSpecs.cs
using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Search.v1;

/// <summary>
/// Specification for searching report definitions.
/// </summary>
public sealed class SearchReportDefinitionsSpecs : Specification<ReportDefinition, ReportDefinitionResponse>
{
    public SearchReportDefinitionsSpecs(SearchReportDefinitionsCommand command)
    {
        Query.OrderBy(r => r.Name);

        if (!string.IsNullOrEmpty(command.Name))
        {
            Query.Where(r => r.Name.Contains(command.Name));
        }

        if (!string.IsNullOrEmpty(command.Code))
        {
            Query.Where(r => r.Code.Contains(command.Code));
        }

        if (!string.IsNullOrEmpty(command.Category))
        {
            Query.Where(r => r.Category == command.Category);
        }

        if (!string.IsNullOrEmpty(command.Status))
        {
            Query.Where(r => r.Status == command.Status);
        }

        if (!string.IsNullOrEmpty(command.OutputFormat))
        {
            Query.Where(r => r.OutputFormat == command.OutputFormat);
        }

        if (command.IsScheduled.HasValue)
        {
            Query.Where(r => r.IsScheduled == command.IsScheduled.Value);
        }

        Query.Skip(command.PageSize * (command.PageNumber - 1))
            .Take(command.PageSize);

        Query.Select(r => new ReportDefinitionResponse(
            r.Id,
            r.Code,
            r.Name,
            r.Category,
            r.OutputFormat,
            r.Description,
            r.ParametersDefinition,
            r.IsScheduled,
            r.ScheduleFrequency,
            r.ScheduleDay,
            r.ScheduleTime,
            r.LastGeneratedAt,
            r.Status,
            r.CreatedOn));
    }
}

