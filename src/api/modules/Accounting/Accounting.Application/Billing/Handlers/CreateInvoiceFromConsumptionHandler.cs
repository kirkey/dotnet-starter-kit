using Accounting.Application.Billing.Commands;

namespace Accounting.Application.Billing.Handlers;

public sealed class CreateInvoiceFromConsumptionHandler(
    IBillingService billingService,
    [FromKeyedServices("accounting:Consumption")] IReadRepository<Consumption> consumptionRepo,
    [FromKeyedServices("accounting:members")] IRepository<Member> memberRepo,
    [FromKeyedServices("accounting:rateschedules")] IReadRepository<RateSchedule> rateRepo,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> invoiceRepo,
    [FromKeyedServices("accounting:accounts")] IReadRepository<ChartOfAccount> accountRepo,
    [FromKeyedServices("accounting:postingbatches")] IRepository<PostingBatch> postingBatchRepo
) : IRequestHandler<CreateInvoiceFromConsumptionCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateInvoiceFromConsumptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var consumption = await consumptionRepo.GetByIdAsync(request.ConsumptionId, cancellationToken);
        if (consumption == null) throw new KeyNotFoundException($"Consumption data {request.ConsumptionId} not found");

        Member? member = null;
        // consumption.MeterId is DefaultIdType (non-nullable); find a member by matching MeterId
        member = (await memberRepo.ListAsync(cancellationToken: cancellationToken)).FirstOrDefault(m => m.MeterId.HasValue && m.MeterId.Value == consumption.MeterId);

        RateSchedule? rateSchedule = null;
        if (member is { RateScheduleId: not null })
        {
            rateSchedule = await rateRepo.GetByIdAsync(member.RateScheduleId.Value, cancellationToken);
        }
        // Fallback: try to find default rate schedule (first one) if none set
        if (rateSchedule == null)
        {
            var rates = await rateRepo.ListAsync(cancellationToken: cancellationToken);
            rateSchedule = rates.FirstOrDefault();
            if (rateSchedule == null) throw new InvalidOperationException("No rate schedule available to bill consumption");
        }

        var draft = billingService.CalculateInvoiceDraft(consumption, rateSchedule);

        // Compose invoice
        var invoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}-{DefaultIdType.NewGuid().ToString().Substring(0,6)}";
        var dueDate = request.InvoiceDate.AddDays(request.DueDays);

        var invoice = Invoice.Create(invoiceNumber,
            member?.Id ?? DefaultIdType.Empty,
            request.InvoiceDate,
            dueDate,
            consumption.Id,
            draft.UsageCharge,
            draft.FixedCharge,
            0m, // tax
            0m, // other charges
            consumption.KWhUsed,
            consumption.BillingPeriod,
            description: $"Auto-generated from consumption {consumption.Id}",
            notes: null);

        // add line items from draft
        foreach (var line in draft.Lines)
        {
            invoice.AddLineItem(line.Description, line.Quantity, line.UnitPrice);
        }

        // mark invoice as Sent
        invoice.Send();

        await invoiceRepo.AddAsync(invoice, cancellationToken);
        await invoiceRepo.SaveChangesAsync(cancellationToken);

        // Create PostingBatch and JournalEntry to post to GL (debit AR, credit Revenue)
        // Find AR and Revenue accounts
        var accounts = await accountRepo.ListAsync(cancellationToken: cancellationToken);
        var arAccount = accounts.FirstOrDefault(a => string.Equals(a.AccountType, "Asset", StringComparison.OrdinalIgnoreCase));
        var revenueAccount = accounts.FirstOrDefault(a => string.Equals(a.AccountType, "Revenue", StringComparison.OrdinalIgnoreCase));
        if (arAccount == null || revenueAccount == null)
        {
            // If accounts not found, skip GL posting but log/raise if needed
        }
        else
        {
            var je = JournalEntry.Create(request.InvoiceDate, invoice.InvoiceNumber, invoice.Description ?? "Invoice Posting", "Billing", null, invoice.TotalAmount);
            // Debit AR
            je.AddLine(arAccount.Id, invoice.TotalAmount, 0m, $"AR for {invoice.InvoiceNumber}");
            // Credit Revenue
            je.AddLine(revenueAccount.Id, 0m, invoice.TotalAmount, $"Revenue for {invoice.InvoiceNumber}");

            var batchNumber = $"BATCH-{DateTime.UtcNow:yyyyMMddHHmmss}-{DefaultIdType.NewGuid().ToString().Substring(0,6)}";
            var batch = PostingBatch.Create(batchNumber, DateTime.UtcNow, description: $"Invoice posting for {invoice.InvoiceNumber}", periodId: null);
            batch.AddJournalEntry(je);

            await postingBatchRepo.AddAsync(batch, cancellationToken);
            await postingBatchRepo.SaveChangesAsync(cancellationToken);
        }

        return invoice.Id;
    }
}
