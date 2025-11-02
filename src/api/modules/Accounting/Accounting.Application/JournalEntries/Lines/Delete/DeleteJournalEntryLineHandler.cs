namespace Accounting.Application.JournalEntries.Lines.Delete;


/// <summary>
/// Handler for deleting a journal entry line.
/// </summary>
public sealed class DeleteJournalEntryLineHandler(
    [FromKeyedServices("accounting:journals")] IReadRepository<JournalEntry> journalEntryRepository,
    [FromKeyedServices("accounting:journal-lines")] IRepository<JournalEntryLine> repository)
    : IRequestHandler<DeleteJournalEntryLineCommand>
{
    public async Task Handle(DeleteJournalEntryLineCommand request, CancellationToken cancellationToken)
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

        line.Delete();
        
        await repository.DeleteAsync(line, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
