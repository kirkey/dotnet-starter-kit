using Accounting.Application.Bills.Queries;

namespace Accounting.Application.Bills.Create.v1;

/// <summary>
/// Handler for creating a new bill.
/// </summary>
public sealed class BillCreateHandler(
    ILogger<BillCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Bill> repository)
    : IRequestHandler<BillCreateCommand, BillCreateResponse>
{
    public async Task<BillCreateResponse> Handle(BillCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate bill number
        var existingByNumber = await repository.FirstOrDefaultAsync(
            new BillByNumberSpec(request.BillNumber), cancellationToken);
        if (existingByNumber != null)
        {
            throw new DuplicateBillNumberException(request.BillNumber);
        }

        var bill = Bill.Create(
            billNumber: request.BillNumber,
            vendorId: request.VendorId,
            vendorInvoiceNumber: request.VendorInvoiceNumber,
            billDate: request.BillDate,
            dueDate: request.DueDate,
            subtotalAmount: request.SubtotalAmount,
            taxAmount: request.TaxAmount,
            shippingAmount: request.ShippingAmount,
            paymentTerms: request.PaymentTerms,
            discountAmount: request.DiscountAmount,
            discountDate: request.DiscountDate,
            purchaseOrderId: request.PurchaseOrderId,
            expenseAccountId: request.ExpenseAccountId,
            periodId: request.PeriodId,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(bill, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bill created {BillId} - {BillNumber}", bill.Id, bill.BillNumber);
        return new BillCreateResponse(bill.Id);
    }
}

