using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Search.v1;

public class SearchFeeDefinitionsSpecs : EntitiesByPaginationFilterSpec<FeeDefinition, FeeDefinitionResponse>
{
    public SearchFeeDefinitionsSpecs(SearchFeeDefinitionsCommand command)
        : base(command) =>
        Query
            .OrderBy(fd => fd.Name, !command.HasOrderBy())
            .Where(fd => fd.Code == command.Code, !string.IsNullOrWhiteSpace(command.Code))
            .Where(fd => fd.Name.Contains(command.Name!), !string.IsNullOrWhiteSpace(command.Name))
            .Where(fd => fd.FeeType == command.FeeType, !string.IsNullOrWhiteSpace(command.FeeType))
            .Where(fd => fd.CalculationType == command.CalculationType, !string.IsNullOrWhiteSpace(command.CalculationType))
            .Where(fd => fd.AppliesTo == command.AppliesTo, !string.IsNullOrWhiteSpace(command.AppliesTo))
            .Where(fd => fd.IsActive == command.IsActive!.Value, command.IsActive.HasValue);
}
