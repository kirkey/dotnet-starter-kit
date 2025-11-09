namespace Accounting.Application.Invoices.Search.v1;

/// <summary>
/// Specification for searching invoices with various filters.
/// </summary>
public class SearchInvoicesSpec : Specification<Invoice>
{
    public SearchInvoicesSpec(SearchInvoicesRequest request)
    {
        Query.Include(i => i.LineItems);

        // Filter by member
        if (request.MemberId.HasValue)
        {
            Query.Where(i => i.MemberId == request.MemberId.Value);
        }

        // Filter by invoice number (partial match)
        if (!string.IsNullOrWhiteSpace(request.InvoiceNumber))
        {
            Query.Where(i => i.InvoiceNumber.Contains(request.InvoiceNumber));
        }

        // Filter by status
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(i => i.Status == request.Status);
        }

        // Filter by invoice date range
        if (request.InvoiceDateFrom.HasValue)
        {
            Query.Where(i => i.InvoiceDate >= request.InvoiceDateFrom.Value);
        }

        if (request.InvoiceDateTo.HasValue)
        {
            Query.Where(i => i.InvoiceDate <= request.InvoiceDateTo.Value);
        }

        // Filter by due date range
        if (request.DueDateFrom.HasValue)
        {
            Query.Where(i => i.DueDate >= request.DueDateFrom.Value);
        }

        if (request.DueDateTo.HasValue)
        {
            Query.Where(i => i.DueDate <= request.DueDateTo.Value);
        }

        // Filter by billing period
        if (!string.IsNullOrWhiteSpace(request.BillingPeriod))
        {
            Query.Where(i => i.BillingPeriod == request.BillingPeriod);
        }

        // Filter by consumption ID
        if (request.ConsumptionId.HasValue)
        {
            Query.Where(i => i.ConsumptionId == request.ConsumptionId.Value);
        }

        // Filter by rate schedule
        if (!string.IsNullOrWhiteSpace(request.RateSchedule))
        {
            Query.Where(i => i.RateSchedule == request.RateSchedule);
        }

        // Filter by amount range
        if (request.MinAmount.HasValue)
        {
            Query.Where(i => i.TotalAmount >= request.MinAmount.Value);
        }

        if (request.MaxAmount.HasValue)
        {
            Query.Where(i => i.TotalAmount <= request.MaxAmount.Value);
        }

        // Filter by outstanding balance
        if (request.HasOutstandingBalance == true)
        {
            Query.Where(i => i.PaidAmount < i.TotalAmount);
        }


        // Default ordering by invoice date descending
        Query.OrderByDescending(i => i.InvoiceDate)
             .ThenByDescending(i => i.CreatedOn);
    }
}

