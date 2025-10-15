using Accounting.Application.BankReconciliations.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.BankReconciliations.Specs;

/// <summary>
/// Specification to retrieve a bank reconciliation by ID projected to <see cref="BankReconciliationResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetBankReconciliationSpec : Specification<BankReconciliation, BankReconciliationResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetBankReconciliationSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the bank reconciliation to retrieve.</param>
    public GetBankReconciliationSpec(DefaultIdType id)
    {
        Query.Where(b => b.Id == id);
    }
}

