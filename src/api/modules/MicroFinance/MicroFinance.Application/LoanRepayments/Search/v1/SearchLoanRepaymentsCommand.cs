using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Search.v1;

/// <summary>
/// Command to search loan repayments with filters and pagination.
/// </summary>
public class SearchLoanRepaymentsCommand : PaginationFilter, IRequest<PagedList<LoanRepaymentResponse>>
{
    /// <summary>
    /// Filter by loan ID.
    /// </summary>
    public Guid? LoanId { get; set; }

    /// <summary>
    /// Filter by payment method.
    /// </summary>
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// Filter by repayment date range start.
    /// </summary>
    public DateOnly? RepaymentDateFrom { get; set; }

    /// <summary>
    /// Filter by repayment date range end.
    /// </summary>
    public DateOnly? RepaymentDateTo { get; set; }

    /// <summary>
    /// Filter by minimum total amount.
    /// </summary>
    public decimal? MinAmount { get; set; }

    /// <summary>
    /// Filter by maximum total amount.
    /// </summary>
    public decimal? MaxAmount { get; set; }
}
