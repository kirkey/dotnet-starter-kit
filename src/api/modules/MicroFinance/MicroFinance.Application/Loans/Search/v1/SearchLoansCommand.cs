using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Search.v1;

public class SearchLoansCommand : PaginationFilter, IRequest<PagedList<LoanSummaryResponse>>
{
    public Guid? MemberId { get; set; }
    public Guid? LoanProductId { get; set; }
    public string? LoanNumber { get; set; }
    public string? Status { get; set; }
    public DateOnly? ApplicationDateFrom { get; set; }
    public DateOnly? ApplicationDateTo { get; set; }
    public DateOnly? DisbursementDateFrom { get; set; }
    public DateOnly? DisbursementDateTo { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public bool? HasOutstandingBalance { get; set; }
}

public sealed record LoanSummaryResponse(
    Guid Id,
    Guid MemberId,
    string MemberName,
    string MemberNumber,
    string LoanProductName,
    string LoanNumber,
    decimal PrincipalAmount,
    decimal InterestRate,
    int TermMonths,
    DateOnly ApplicationDate,
    DateOnly? DisbursementDate,
    decimal OutstandingPrincipal,
    decimal OutstandingInterest,
    string Status
);
