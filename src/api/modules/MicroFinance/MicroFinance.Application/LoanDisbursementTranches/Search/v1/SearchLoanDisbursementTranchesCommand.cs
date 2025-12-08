using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Search.v1;

public class SearchLoanDisbursementTranchesCommand : PaginationFilter, IRequest<PagedList<LoanDisbursementTrancheSummaryResponse>>
{
    public DefaultIdType? LoanId { get; set; }
    public string? TrancheNumber { get; set; }
    public string? DisbursementMethod { get; set; }
    public string? Status { get; set; }
    public DateOnly? ScheduledDateFrom { get; set; }
    public DateOnly? ScheduledDateTo { get; set; }
    public DateOnly? DisbursedDateFrom { get; set; }
    public DateOnly? DisbursedDateTo { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
}

public sealed record LoanDisbursementTrancheSummaryResponse(
    DefaultIdType Id,
    DefaultIdType LoanId,
    int TrancheSequence,
    string TrancheNumber,
    DateOnly ScheduledDate,
    DateOnly? DisbursedDate,
    decimal Amount,
    decimal Deductions,
    decimal NetAmount,
    string DisbursementMethod,
    string? BankAccountNumber,
    string? BankName,
    string Status
);
