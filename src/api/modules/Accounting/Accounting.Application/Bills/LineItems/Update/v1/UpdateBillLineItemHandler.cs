using Accounting.Application.Bills.LineItems.GetList.v1;

namespace Accounting.Application.Bills.LineItems.Update.v1;

/// <summary>
/// Handler for updating a bill line item.
/// Validates that the bill exists and is not posted or paid before updating.
/// Automatically recalculates the bill total after updating the line item.
/// </summary>
public sealed class UpdateBillLineItemHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> billRepository,
    [FromKeyedServices("accounting:bill-line-items")] IRepository<BillLineItem> lineItemRepository,
    ILogger<UpdateBillLineItemHandler> logger)
    : IRequestHandler<UpdateBillLineItemCommand, UpdateBillLineItemResponse>
{
    public async Task<UpdateBillLineItemResponse> Handle(UpdateBillLineItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Updating line item {LineItemId}", request.LineItemId);

        // Verify bill exists and is not posted/paid
        var bill = await billRepository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        if (bill.IsPosted)
            throw new BillCannotBeModifiedException(request.BillId, "Bill is posted");

        if (bill.IsPaid)
            throw new BillCannotBeModifiedException(request.BillId, "Bill is paid");

        // Get and update line item
        var lineItem = await lineItemRepository.GetByIdAsync(request.LineItemId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillLineItemNotFoundException(request.LineItemId);

        lineItem.Update(
            request.LineNumber,
            request.Description,
            request.Quantity,
            request.UnitPrice,
            request.Amount,
            request.ChartOfAccountId,
            request.TaxCodeId,
            request.TaxAmount,
            request.ProjectId,
            request.CostCenterId,
            request.Notes);

        await lineItemRepository.UpdateAsync(lineItem, cancellationToken).ConfigureAwait(false);
        await lineItemRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // Recalculate bill total
        var spec = new GetBillLineItemsByBillIdSpec(request.BillId);
        var allLineItems = await lineItemRepository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalAmount = allLineItems.Sum(li => li.Amount);

        bill.UpdateTotalAmount(totalAmount);
        await billRepository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        await billRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Line item {LineItemId} updated. Bill total updated to {TotalAmount}",
            request.LineItemId, totalAmount);

        return new UpdateBillLineItemResponse(lineItem.Id);
    }
}
