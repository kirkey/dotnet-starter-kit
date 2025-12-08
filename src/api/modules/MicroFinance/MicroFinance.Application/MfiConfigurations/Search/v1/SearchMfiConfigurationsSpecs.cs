using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Search.v1;

public class SearchMfiConfigurationsSpecs : EntitiesByPaginationFilterSpec<MfiConfiguration, MfiConfigurationSummaryResponse>
{
    public SearchMfiConfigurationsSpecs(SearchMfiConfigurationsCommand command)
        : base(command) =>
        Query
            .OrderBy(x => x.Category, !command.HasOrderBy())
            .ThenBy(x => x.DisplayOrder)
            .Where(x => x.Key.Contains(command.Key!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.Key))
            .Where(x => x.Category == command.Category, !string.IsNullOrWhiteSpace(command.Category))
            .Where(x => x.DataType == command.DataType, !string.IsNullOrWhiteSpace(command.DataType))
            .Where(x => x.IsEditable == command.IsEditable!.Value, command.IsEditable.HasValue)
            .Where(x => x.IsEncrypted == command.IsEncrypted!.Value, command.IsEncrypted.HasValue)
            .Where(x => x.BranchId == command.BranchId, command.BranchId.HasValue);
}
