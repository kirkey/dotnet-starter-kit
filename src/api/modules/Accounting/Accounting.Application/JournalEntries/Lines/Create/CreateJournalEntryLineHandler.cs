namespace Accounting.Application.JournalEntries.Lines.Create;


/// <summary>
/// Handler for creating a new journal entry line.
/// </summary>
public sealed class CreateJournalEntryLineHandler(
    ILogger<CreateJournalEntryLineHandler> logger,
    [FromKeyedServices("accounting:journals")] IReadRepository<JournalEntry> journalEntryRepository,
    [FromKeyedServices("accounting:journal-lines")] IRepository<JournalEntryLine> repository)
    : IRequestHandler<CreateJournalEntryLineCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateJournalEntryLineCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify parent journal entry exists
        var journalEntry = await journalEntryRepository.GetByIdAsync(request.JournalEntryId, cancellationToken);
        if (journalEntry == null)
            throw new JournalEntryNotFoundException(request.JournalEntryId);

        // Verify journal entry is not posted
        if (journalEntry.IsPosted)
            throw new JournalEntryCannotBeModifiedException(request.JournalEntryId);

        var line = JournalEntryLine.Create(
            request.JournalEntryId,
            request.AccountId,
            request.DebitAmount,
            request.CreditAmount,
            request.Memo,
            request.Reference);

        await repository.AddAsync(line, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("journal entry line created {JournalEntryLineId} for journal entry {JournalEntryId}", 
            line.Id, request.JournalEntryId);

        return line.Id;
    }
}

