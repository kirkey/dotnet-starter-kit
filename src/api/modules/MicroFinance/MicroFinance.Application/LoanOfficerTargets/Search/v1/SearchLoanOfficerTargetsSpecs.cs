using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Search.v1;

public class SearchLoanOfficerTargetsSpecs : EntitiesByPaginationFilterSpec<LoanOfficerTarget, LoanOfficerTargetSummaryResponse>
{
    public SearchLoanOfficerTargetsSpecs(SearchLoanOfficerTargetsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.PeriodStart, !command.HasOrderBy())
            .Where(x => x.StaffId == command.StaffId!.Value, command.StaffId.HasValue)
            .Where(x => x.TargetType == command.TargetType, !string.IsNullOrWhiteSpace(command.TargetType))
            .Where(x => x.Period == command.Period, !string.IsNullOrWhiteSpace(command.Period))
            .Where(x => x.PeriodStart >= command.PeriodStartFrom!.Value, command.PeriodStartFrom.HasValue)
            .Where(x => x.PeriodStart <= command.PeriodStartTo!.Value, command.PeriodStartTo.HasValue)
            .Where(x => x.PeriodEnd >= command.PeriodEndFrom!.Value, command.PeriodEndFrom.HasValue)
            .Where(x => x.PeriodEnd <= command.PeriodEndTo!.Value, command.PeriodEndTo.HasValue)
            .Where(x => x.TargetValue >= command.TargetValueMin!.Value, command.TargetValueMin.HasValue)
            .Where(x => x.TargetValue <= command.TargetValueMax!.Value, command.TargetValueMax.HasValue);
}
