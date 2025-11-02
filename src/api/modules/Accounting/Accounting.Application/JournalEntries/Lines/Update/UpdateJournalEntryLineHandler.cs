namespace Accounting.Application.JournalEntries.Lines.Update;


/// <summary>
/// Handler for updating an existing journal entry line.
/// </summary>
public sealed class UpdateJournalEntryLineHandler(
    [FromKeyedServices("accounting:journals")] IReadRepository<JournalEntry> journalEntryRepository,
    [FromKeyedServices("accounting:journal-lines")] IRepository<JournalEntryLine> repository)
    : IRequestHandler<UpdateJournalEntryLineCommand>
{
    public async Task Handle(UpdateJournalEntryLineCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var line = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (line == null)
            throw new JournalEntryLineNotFoundException(request.Id);

        // Verify parent journal entry is not posted
        var journalEntry = await journalEntryRepository.GetByIdAsync(line.JournalEntryId, cancellationToken);
        if (journalEntry == null)
            throw new JournalEntryNotFoundException(line.JournalEntryId);

        if (journalEntry.IsPosted)
            throw new JournalEntryCannotBeModifiedException(journalEntry.Id);

        line.Update(request.DebitAmount, request.CreditAmount, request.Memo, request.Reference);

        await repository.UpdateAsync(line, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

