using Accounting.Application.Patronages.Commands;

namespace Accounting.Application.Patronages.Handlers;

public sealed class RetirePatronageHandler(
    [FromKeyedServices("accounting:patronagecapitals")] IRepository<PatronageCapital> patronageRepo,
    [FromKeyedServices("accounting:accounts")] IReadRepository<ChartOfAccount> accountRepo,
    [FromKeyedServices("accounting:postingbatches")] IRepository<PostingBatch> postingBatchRepo,
    [FromKeyedServices("accounting:journal-lines")] IRepository<JournalEntryLine> journalLineRepo)
    : IRequestHandler<RetirePatronageCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(RetirePatronageCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var patronage = await patronageRepo.GetByIdAsync(request.PatronageCapitalId, cancellationToken);
        _ = patronage ?? throw new PatronageCapitalByIdNotFoundException(request.PatronageCapitalId);

        if (request.Amount <= 0) throw new ArgumentException("Retirement amount must be positive");

        // Update domain
        patronage.Retire(request.Amount);
        await patronageRepo.UpdateAsync(patronage, cancellationToken);

        // Create GL posting: Debit PatronageCapital (equity decrease), Credit Cash/Bank (asset decrease)
        var accounts = await accountRepo.ListAsync(cancellationToken: cancellationToken);
        var equityAccount = accounts.FirstOrDefault(a => string.Equals(a.AccountType, "Equity", StringComparison.OrdinalIgnoreCase));
        var cashAccount = accounts.FirstOrDefault(a => string.Equals(a.AccountType, "Asset", StringComparison.OrdinalIgnoreCase));

        if (equityAccount != null && cashAccount != null)
        {
            var je = JournalEntry.Create(request.RetirementDate, $"RET-{patronage.Id}", request.Description ?? "Patronage retirement", "Patronage", null, request.Amount);

            var batchNumber = $"RET-{DateTime.UtcNow:yyyyMMddHHmmss}-{DefaultIdType.NewGuid().ToString().Substring(0,6)}";
            var batch = PostingBatch.Create(batchNumber, DateTime.UtcNow, description: $"Patronage retirement {patronage.Id}", periodId: null);
            batch.AddJournalEntry(je);

            await postingBatchRepo.AddAsync(batch, cancellationToken);
            await postingBatchRepo.SaveChangesAsync(cancellationToken);
            
            // Create journal entry lines separately (new pattern)
            // Debit Equity (to reduce equity)
            var equityLine = JournalEntryLine.Create(je.Id, equityAccount.Id, request.Amount, 0m, $"Retire patronage {patronage.Id}");
            await journalLineRepo.AddAsync(equityLine, cancellationToken);
            
            // Credit Cash
            var cashLine = JournalEntryLine.Create(je.Id, cashAccount.Id, 0m, request.Amount, $"Pay patronage {patronage.Id}");
            await journalLineRepo.AddAsync(cashLine, cancellationToken);
            
            await journalLineRepo.SaveChangesAsync(cancellationToken);
        }

        await patronageRepo.SaveChangesAsync(cancellationToken);
        return patronage.Id;
    }
}

