namespace Accounting.Application.JournalEntries.Reverse;

/// <summary>
/// Handler for reversing a posted journal entry.
/// Creates a new journal entry with opposite debit/credit amounts.
/// </summary>
public sealed class ReverseJournalEntryHandler(
    ILogger<ReverseJournalEntryHandler> logger,
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository,
    [FromKeyedServices("accounting:journal-lines")] IRepository<JournalEntryLine> journalLineRepo)
    : IRequestHandler<ReverseJournalEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ReverseJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Use spec to load entry with lines in one query
        var spec = new Specs.GetJournalEntryWithLinesSpec(request.JournalEntryId);
        var originalEntry = await ((IReadRepository<JournalEntry>)repository).FirstOrDefaultAsync(spec, cancellationToken);
        
        if (originalEntry == null)
        {
            throw new JournalEntryNotFoundException(request.JournalEntryId);
        }

        if (!originalEntry.IsPosted)
        {
            throw new InvalidOperationException($"Cannot reverse journal entry {request.JournalEntryId} because it is not posted.");
        }

        // Create the reversing entry
        var reversingEntry = JournalEntry.Create(
            request.ReversalDate,
            $"REV-{originalEntry.ReferenceNumber}",
            $"REVERSAL: {request.ReversalReason}",
            $"Reversal-{originalEntry.Source}",
            originalEntry.PeriodId,
            originalEntry.OriginalAmount);

        await repository.AddAsync(reversingEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        // Create reversed lines (swap debits and credits) from the loaded lines
        foreach (var line in originalEntry.Lines)
        {
            var reversedLine = JournalEntryLine.Create(
                reversingEntry.Id,
                line.AccountId,
                line.CreditAmount,  // Swap: credit becomes debit
                line.DebitAmount,   // Swap: debit becomes credit
                $"Reversal of: {line.Memo}",
                line.Reference);
            await journalLineRepo.AddAsync(reversedLine, cancellationToken);
        }
        
        await journalLineRepo.SaveChangesAsync(cancellationToken);

        // Post the reversing entry automatically
        reversingEntry.Post();
        await repository.UpdateAsync(reversingEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        // Queue the reversal event on the original entry
        originalEntry.Reverse(request.ReversalDate, request.ReversalReason);
        await repository.UpdateAsync(originalEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Journal entry {OriginalEntryId} reversed with new entry {ReversingEntryId} on {ReversalDate}. Reason: {Reason}", 
            request.JournalEntryId, reversingEntry.Id, request.ReversalDate, request.ReversalReason);

        return reversingEntry.Id;
    }
}

