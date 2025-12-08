using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Search.v1;

public class SearchDebtSettlementsSpecs : EntitiesByPaginationFilterSpec<DebtSettlement, DebtSettlementSummaryResponse>
{
    public SearchDebtSettlementsSpecs(SearchDebtSettlementsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.ProposedDate, !command.HasOrderBy())
            .Where(x => x.ReferenceNumber == command.ReferenceNumber, !string.IsNullOrWhiteSpace(command.ReferenceNumber))
            .Where(x => x.CollectionCaseId == command.CollectionCaseId!.Value, command.CollectionCaseId.HasValue)
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.SettlementType == command.SettlementType, !string.IsNullOrWhiteSpace(command.SettlementType))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.ProposedDate >= command.ProposedDateFrom!.Value, command.ProposedDateFrom.HasValue)
            .Where(x => x.ProposedDate <= command.ProposedDateTo!.Value, command.ProposedDateTo.HasValue)
            .Where(x => x.DueDate >= command.DueDateFrom!.Value, command.DueDateFrom.HasValue)
            .Where(x => x.DueDate <= command.DueDateTo!.Value, command.DueDateTo.HasValue);
}
