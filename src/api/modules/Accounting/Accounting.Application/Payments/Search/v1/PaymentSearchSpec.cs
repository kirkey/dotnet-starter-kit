namespace Accounting.Application.Payments.Search.v1;

/// <summary>
/// Specification for searching payments.
/// </summary>
public sealed class PaymentSearchSpec : Specification<Payment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentSearchSpec"/> class.
    /// </summary>
    public PaymentSearchSpec(PaymentSearchRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        // Filter by payment number
        if (!string.IsNullOrWhiteSpace(query.PaymentNumber))
        {
            Query.Where(p => p.PaymentNumber.Contains(query.PaymentNumber));
        }

        // Filter by member ID
        if (query.MemberId.HasValue && query.MemberId.Value != DefaultIdType.Empty)
        {
            Query.Where(p => p.MemberId == query.MemberId.Value);
        }

        // Filter by payment method
        if (!string.IsNullOrWhiteSpace(query.PaymentMethod))
        {
            Query.Where(p => p.PaymentMethod == query.PaymentMethod);
        }

        // Filter by date range
        if (query.StartDate.HasValue)
        {
            Query.Where(p => p.PaymentDate >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            Query.Where(p => p.PaymentDate <= query.EndDate.Value);
        }

        // Filter by amount range
        if (query.MinAmount.HasValue)
        {
            Query.Where(p => p.Amount >= query.MinAmount.Value);
        }

        if (query.MaxAmount.HasValue)
        {
            Query.Where(p => p.Amount <= query.MaxAmount.Value);
        }

        // Filter by unapplied amount
        if (query.HasUnappliedAmount.HasValue)
        {
            if (query.HasUnappliedAmount.Value)
            {
                Query.Where(p => p.UnappliedAmount > 0);
            }
            else
            {
                Query.Where(p => p.UnappliedAmount == 0);
            }
        }


        // Order by payment date descending, then by payment number
        Query.OrderByDescending(p => p.PaymentDate)
             .ThenBy(p => p.PaymentNumber);
    }
}

