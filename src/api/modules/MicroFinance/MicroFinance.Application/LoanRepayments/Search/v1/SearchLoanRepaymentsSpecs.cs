using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Search.v1;

/// <summary>
/// Specification for searching loan repayments with filters and pagination.
/// </summary>
public class SearchLoanRepaymentsSpecs : EntitiesByPaginationFilterSpec<LoanRepayment, LoanRepaymentResponse>
{
    public SearchLoanRepaymentsSpecs(SearchLoanRepaymentsCommand command)
        : base(command) =>
        Query
            .Include(r => r.Loan)
            .ThenInclude(l => l.Member)
            .OrderByDescending(r => r.RepaymentDate, !command.HasOrderBy())
            .Where(r => r.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(r => r.PaymentMethod == command.PaymentMethod, !string.IsNullOrEmpty(command.PaymentMethod))
            .Where(r => r.RepaymentDate >= command.RepaymentDateFrom!.Value, command.RepaymentDateFrom.HasValue)
            .Where(r => r.RepaymentDate <= command.RepaymentDateTo!.Value, command.RepaymentDateTo.HasValue)
            .Where(r => (r.PrincipalAmount + r.InterestAmount + r.PenaltyAmount) >= command.MinAmount!.Value, command.MinAmount.HasValue)
            .Where(r => (r.PrincipalAmount + r.InterestAmount + r.PenaltyAmount) <= command.MaxAmount!.Value, command.MaxAmount.HasValue);
}
