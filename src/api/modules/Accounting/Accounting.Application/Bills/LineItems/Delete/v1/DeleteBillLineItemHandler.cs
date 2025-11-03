using Accounting.Application.Bills.LineItems.GetList.v1;

namespace Accounting.Application.Bills.LineItems.Delete.v1;

/// <summary>
/// Handler for deleting a bill line item.
/// Validates that the bill exists and is not posted or paid before deleting.
/// Automatically recalculates the bill total after deleting the line item.
/// </summary>
public sealed class DeleteBillLineItemHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> billRepository,
    [FromKeyedServices("accounting:billlineitems")] IRepository<BillLineItem> lineItemRepository,
    ILogger<DeleteBillLineItemHandler> logger)
    : IRequestHandler<DeleteBillLineItemCommand, DeleteBillLineItemResponse>
{
    public async Task<DeleteBillLineItemResponse> Handle(DeleteBillLineItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Deleting line item {LineItemId}", request.LineItemId);

        // Verify bill exists and is not posted/paid
        var bill = await billRepository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        if (bill.IsPosted)
            throw new BillCannotBeModifiedException(request.BillId, "Cannot delete line items from posted bill");

        if (bill.IsPaid)
            throw new BillCannotBeModifiedException(request.BillId, "Cannot delete line items from paid bill");

        // Get and delete line item
        var lineItem = await lineItemRepository.GetByIdAsync(request.LineItemId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillLineItemNotFoundException(request.LineItemId);

        await lineItemRepository.DeleteAsync(lineItem, cancellationToken).ConfigureAwait(false);
        await lineItemRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // Recalculate bill total
        var spec = new GetBillLineItemsByBillIdSpec(request.BillId);
        var allLineItems = await lineItemRepository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalAmount = allLineItems.Sum(li => li.Amount);

        bill.UpdateTotalAmount(totalAmount);
        await billRepository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        await billRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Line item {LineItemId} deleted. Bill total updated to {TotalAmount}",
            request.LineItemId, totalAmount);

        return new DeleteBillLineItemResponse(request.LineItemId);
    }
}
