using Accounting.Application.BankReconciliations.Responses;

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

        Query.Select(r => new BankReconciliationResponse
        {
            Id = r.Id,
            BankAccountId = r.BankAccountId,
            ReconciliationDate = r.ReconciliationDate,
            StatementBalance = r.StatementBalance,
            BookBalance = r.BookBalance,
            AdjustedBalance = r.AdjustedBalance,
            OutstandingChecksTotal = r.OutstandingChecksTotal,
            DepositsInTransitTotal = r.DepositsInTransitTotal,
            BankErrors = r.BankErrors,
            BookErrors = r.BookErrors,
            Status = r.Status.ToString(),
            IsReconciled = r.IsReconciled,
            ReconciledDate = r.ReconciledDate,
            ReconciledBy = r.ReconciledBy,
            ApprovedBy = r.ApprovedBy,
            ApprovedDate = r.ApprovedDate,
            StatementNumber = r.StatementNumber,
            Description = r.Description,
            Notes = r.Notes,
            CreatedOn = r.CreatedOn,
            CreatedBy = r.CreatedBy,
            LastModifiedOn = r.LastModifiedOn,
            LastModifiedBy = r.LastModifiedBy
        });
    }
}
