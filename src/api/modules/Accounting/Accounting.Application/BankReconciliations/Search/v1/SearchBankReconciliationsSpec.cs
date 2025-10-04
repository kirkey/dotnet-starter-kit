using Accounting.Application.BankReconciliations.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.BankReconciliations.Search.v1;

public class SearchBankReconciliationsSpec : Specification<BankReconciliation, BankReconciliationResponse>
{
    public SearchBankReconciliationsSpec(SearchBankReconciliationsCommand request)
    {
        Query
            .Where(r => !request.BankAccountId.HasValue || r.BankAccountId == request.BankAccountId.Value)
            .Where(r => !request.FromDate.HasValue || r.ReconciliationDate >= request.FromDate.Value)
            .Where(r => !request.ToDate.HasValue || r.ReconciliationDate <= request.ToDate.Value)
            .Where(r => string.IsNullOrEmpty(request.Status) || r.Status.ToString() == request.Status)
            .Where(r => !request.IsReconciled.HasValue || r.IsReconciled == request.IsReconciled.Value)
            .OrderByDescending(r => r.ReconciliationDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}
