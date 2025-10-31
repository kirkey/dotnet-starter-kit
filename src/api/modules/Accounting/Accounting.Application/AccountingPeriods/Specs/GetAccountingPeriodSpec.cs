using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Specs;

/// <summary>
/// Specification to retrieve an accounting period by ID projected to <see cref="AccountingPeriodResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetAccountingPeriodSpec : Specification<AccountingPeriod, AccountingPeriodResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetAccountingPeriodSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the accounting period to retrieve.</param>
    public GetAccountingPeriodSpec(DefaultIdType id)
    {
        Query.Where(p => p.Id == id);
    }
}

