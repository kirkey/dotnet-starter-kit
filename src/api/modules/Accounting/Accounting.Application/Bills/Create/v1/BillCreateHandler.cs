namespace Accounting.Application.Bills.Create.v1;

/// <summary>
/// Handler for creating a new bill with line items.
/// </summary>
public sealed class BillCreateHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> billRepository,
    [FromKeyedServices("accounting:bill-line-items")] IRepository<BillLineItem> lineItemRepository,
    ILogger<BillCreateHandler> logger)
    : IRequestHandler<BillCreateCommand, BillCreateResponse>
{
    public async Task<BillCreateResponse> Handle(BillCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation(
            "Creating bill {BillNumber} for vendor {VendorId} with {LineCount} line items",
            request.BillNumber, request.VendorId, request.LineItems?.Count ?? 0);

        // Create the bill
        var bill = Bill.Create(
            request.BillNumber,
            request.VendorId,
            request.BillDate,
            request.DueDate,
            request.Description,
            request.PeriodId,
            request.PaymentTerms,
            request.PurchaseOrderNumber,
            request.Notes);

        await billRepository.AddAsync(bill, cancellationToken).ConfigureAwait(false);

        // Create line items if provided
        if (request.LineItems != null && request.LineItems.Count > 0)
        {
            decimal totalAmount = 0;

            foreach (var lineDto in request.LineItems)
            {
                var lineItem = BillLineItem.Create(
                    bill.Id,
                    lineDto.LineNumber,
                    lineDto.Description,
                    lineDto.Quantity,
                    lineDto.UnitPrice,
                    lineDto.Amount,
                    lineDto.ChartOfAccountId,
                    lineDto.TaxCodeId,
                    lineDto.TaxAmount,
                    lineDto.ProjectId,
                    lineDto.CostCenterId,
                    lineDto.Notes);

                await lineItemRepository.AddAsync(lineItem, cancellationToken).ConfigureAwait(false);
                totalAmount += lineDto.Amount;
            }

            // Update bill total amount
            bill.UpdateTotalAmount(totalAmount);
            await billRepository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        }

        await billRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Bill {BillNumber} created successfully with ID {BillId}",
            request.BillNumber, bill.Id);

        return new BillCreateResponse(bill.Id);
    }
}

