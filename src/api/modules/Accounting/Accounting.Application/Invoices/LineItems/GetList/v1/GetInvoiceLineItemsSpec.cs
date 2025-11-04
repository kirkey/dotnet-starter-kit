namespace Accounting.Application.Invoices.LineItems.GetList.v1;

/// <summary>
/// Specification to get all line items for a specific invoice.
/// </summary>
public class GetInvoiceLineItemsSpec : Specification<InvoiceLineItem>
{
    public GetInvoiceLineItemsSpec(DefaultIdType invoiceId)
    {
        Query.Where(li => li.InvoiceId == invoiceId)
             .OrderBy(li => li.CreatedOn);
    }
}

