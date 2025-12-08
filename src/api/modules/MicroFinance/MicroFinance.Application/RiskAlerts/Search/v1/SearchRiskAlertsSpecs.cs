using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Search.v1;

public class SearchRiskAlertsSpecs : EntitiesByPaginationFilterSpec<RiskAlert, RiskAlertSummaryResponse>
{
    public SearchRiskAlertsSpecs(SearchRiskAlertsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.AlertedAt, !command.HasOrderBy())
            .Where(x => x.AlertNumber == command.AlertNumber, !string.IsNullOrWhiteSpace(command.AlertNumber))
            .Where(x => x.RiskCategoryId == command.RiskCategoryId!.Value, command.RiskCategoryId.HasValue)
            .Where(x => x.RiskIndicatorId == command.RiskIndicatorId!.Value, command.RiskIndicatorId.HasValue)
            .Where(x => x.Severity == command.Severity, !string.IsNullOrWhiteSpace(command.Severity))
            .Where(x => x.Source == command.Source, !string.IsNullOrWhiteSpace(command.Source))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.BranchId == command.BranchId!.Value, command.BranchId.HasValue)
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.AssignedToUserId == command.AssignedToUserId!.Value, command.AssignedToUserId.HasValue)
            .Where(x => x.AlertedAt >= command.AlertedFrom!.Value, command.AlertedFrom.HasValue)
            .Where(x => x.AlertedAt <= command.AlertedTo!.Value, command.AlertedTo.HasValue);
}
