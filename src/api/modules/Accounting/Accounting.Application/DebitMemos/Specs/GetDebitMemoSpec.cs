using Accounting.Application.DebitMemos.Responses;

namespace Accounting.Application.DebitMemos.Specs;

/// <summary>
/// Specification to retrieve a debit memo by ID projected to <see cref="DebitMemoResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetDebitMemoSpec : Specification<DebitMemo, DebitMemoResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetDebitMemoSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the debit memo to retrieve.</param>
    public GetDebitMemoSpec(DefaultIdType id)
    {
        Query.Where(d => d.Id == id);
    }
}
