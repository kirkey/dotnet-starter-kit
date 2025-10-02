using Accounting.Application.BankReconciliations.Responses;

namespace Accounting.Application.BankReconciliations.Search.v1;

public class SearchBankReconciliationsCommand : PaginationFilter, IRequest<PagedList<BankReconciliationResponse>>
{
    public DefaultIdType? BankAccountId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Status { get; set; }
    public bool? IsReconciled { get; set; }
}
