namespace Accounting.Application.GeneralLedger.Specifications;

public class GeneralLedgerByAccountAndDateSpec : Specification<Domain.GeneralLedger>
{
    public GeneralLedgerByAccountAndDateSpec(DefaultIdType accountId, DateTime? endDate)
    {
        Query.Where(gl => gl.AccountId == accountId);
        
        if (endDate.HasValue)
        {
            Query.Where(gl => gl.TransactionDate <= endDate.Value);
        }
        
        Query.OrderBy(gl => gl.TransactionDate)
             .ThenBy(gl => gl.Id);
    }
}
