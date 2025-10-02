using Accounting.Domain.Entities;

namespace Accounting.Application.GeneralLedgers.Specifications;

public class GeneralLedgerByDateRangeSpec : Specification<GeneralLedger>
{
    public GeneralLedgerByDateRangeSpec(DateTime? startDate, DateTime? endDate)
    {
        if (startDate.HasValue)
        {
            Query.Where(gl => gl.TransactionDate >= startDate.Value);
        }
        
        if (endDate.HasValue)
        {
            Query.Where(gl => gl.TransactionDate <= endDate.Value);
        }
        
        Query.OrderBy(gl => gl.TransactionDate)
            .ThenBy(gl => gl.Id);
    }
}
