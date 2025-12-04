using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Search.v1;

public class SearchLoansSpecs : EntitiesByPaginationFilterSpec<Loan, LoanSummaryResponse>
{
    public SearchLoansSpecs(SearchLoansCommand command)
        : base(command) =>
        Query
            .Include(l => l.Member)
            .Include(l => l.LoanProduct)
            .OrderBy(l => l.LoanNumber, !command.HasOrderBy())
            .Where(l => l.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(l => l.LoanProductId == command.LoanProductId!.Value, command.LoanProductId.HasValue)
            .Where(l => l.LoanNumber == command.LoanNumber, !string.IsNullOrWhiteSpace(command.LoanNumber))
            .Where(l => l.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(l => l.ApplicationDate >= command.ApplicationDateFrom!.Value, command.ApplicationDateFrom.HasValue)
            .Where(l => l.ApplicationDate <= command.ApplicationDateTo!.Value, command.ApplicationDateTo.HasValue)
            .Where(l => l.DisbursementDate >= command.DisbursementDateFrom!.Value, command.DisbursementDateFrom.HasValue)
            .Where(l => l.DisbursementDate <= command.DisbursementDateTo!.Value, command.DisbursementDateTo.HasValue)
            .Where(l => l.PrincipalAmount >= command.MinAmount!.Value, command.MinAmount.HasValue)
            .Where(l => l.PrincipalAmount <= command.MaxAmount!.Value, command.MaxAmount.HasValue)
            .Where(l => l.OutstandingPrincipal > 0 || l.OutstandingInterest > 0, command.HasOutstandingBalance == true)
            .Where(l => l.OutstandingPrincipal == 0 && l.OutstandingInterest == 0, command.HasOutstandingBalance == false);
}
