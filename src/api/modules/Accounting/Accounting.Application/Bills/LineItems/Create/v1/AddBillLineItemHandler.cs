using Accounting.Application.Bills.LineItems.GetList.v1;

namespace Accounting.Application.Bills.LineItems.Create.v1;

/// <summary>
/// Handler for adding a line item to a bill.
/// Validates that the bill exists and is not posted or paid before adding the line item.
/// Automatically recalculates the bill total after adding the line item.
/// </summary>
public sealed class AddBillLineItemHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> billRepository,
    [FromKeyedServices("accounting:bill-line-items")] IRepository<BillLineItem> lineItemRepository,
    ILogger<AddBillLineItemHandler> logger)
    : IRequestHandler<AddBillLineItemCommand, AddBillLineItemResponse>
{
    public async Task<AddBillLineItemResponse> Handle(AddBillLineItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation(
            "Adding line item {LineNumber} to bill {BillId}",
            request.LineNumber, request.BillId);

        // Verify bill exists and is not posted/paid
        var bill = await billRepository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        if (bill.IsPosted)
            throw new BillLineItemCannotBeAddedException(request.BillId);

        if (bill.IsPaid)
            throw new BillLineItemCannotBeAddedException(request.BillId);

        // Create line item
        var lineItem = BillLineItem.Create(
            request.BillId,
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

        await lineItemRepository.AddAsync(lineItem, cancellationToken).ConfigureAwait(false);
        await lineItemRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // Recalculate bill total
        var spec = new GetBillLineItemsByBillIdSpec(request.BillId);
        var allLineItems = await lineItemRepository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalAmount = allLineItems.Sum(li => li.Amount);

        bill.UpdateTotalAmount(totalAmount);
        await billRepository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        await billRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Line item {LineItemId} added to bill {BillId}. Bill total updated to {TotalAmount}",
            lineItem.Id, request.BillId, totalAmount);

        return new AddBillLineItemResponse(lineItem.Id);
    }
}
