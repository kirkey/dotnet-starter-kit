using Accounting.Application.CreditMemos.Responses;

namespace Accounting.Application.CreditMemos.Specs;

/// <summary>
/// Specification to retrieve a credit memo by ID projected to <see cref="CreditMemoResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetCreditMemoSpec : Specification<CreditMemo, CreditMemoResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetCreditMemoSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the credit memo to retrieve.</param>
    public GetCreditMemoSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);
    }
}
