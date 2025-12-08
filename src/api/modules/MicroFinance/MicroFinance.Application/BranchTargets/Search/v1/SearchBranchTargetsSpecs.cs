using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Search.v1;

public class SearchBranchTargetsSpecs : EntitiesByPaginationFilterSpec<BranchTarget, BranchTargetSummaryResponse>
{
    public SearchBranchTargetsSpecs(SearchBranchTargetsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.PeriodStart, !command.HasOrderBy())
            .ThenBy(x => x.TargetType)
            .Where(x => x.BranchId == command.BranchId!.Value, command.BranchId.HasValue)
            .Where(x => x.TargetType == command.TargetType, !string.IsNullOrWhiteSpace(command.TargetType))
            .Where(x => x.Period == command.Period, !string.IsNullOrWhiteSpace(command.Period))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.PeriodStart >= command.PeriodStartFrom!.Value, command.PeriodStartFrom.HasValue)
            .Where(x => x.PeriodStart <= command.PeriodStartTo!.Value, command.PeriodStartTo.HasValue)
            .Where(x => x.PeriodEnd >= command.PeriodEndFrom!.Value, command.PeriodEndFrom.HasValue)
            .Where(x => x.PeriodEnd <= command.PeriodEndTo!.Value, command.PeriodEndTo.HasValue);
}
