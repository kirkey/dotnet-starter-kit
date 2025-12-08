using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Search.v1;

public class SearchLoanDisbursementTranchesSpecs : EntitiesByPaginationFilterSpec<LoanDisbursementTranche, LoanDisbursementTrancheSummaryResponse>
{
    public SearchLoanDisbursementTranchesSpecs(SearchLoanDisbursementTranchesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.ScheduledDate, !command.HasOrderBy())
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.TrancheNumber.Contains(command.TrancheNumber!), !string.IsNullOrWhiteSpace(command.TrancheNumber))
            .Where(x => x.DisbursementMethod == command.DisbursementMethod, !string.IsNullOrWhiteSpace(command.DisbursementMethod))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.ScheduledDate >= command.ScheduledDateFrom!.Value, command.ScheduledDateFrom.HasValue)
            .Where(x => x.ScheduledDate <= command.ScheduledDateTo!.Value, command.ScheduledDateTo.HasValue)
            .Where(x => x.DisbursedDate >= command.DisbursedDateFrom!.Value, command.DisbursedDateFrom.HasValue)
            .Where(x => x.DisbursedDate <= command.DisbursedDateTo!.Value, command.DisbursedDateTo.HasValue)
            .Where(x => x.Amount >= command.MinAmount!.Value, command.MinAmount.HasValue)
            .Where(x => x.Amount <= command.MaxAmount!.Value, command.MaxAmount.HasValue);
}
