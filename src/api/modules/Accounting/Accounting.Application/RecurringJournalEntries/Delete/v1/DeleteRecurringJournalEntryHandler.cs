namespace Accounting.Application.RecurringJournalEntries.Delete.v1;

public sealed class DeleteRecurringJournalEntryHandler(
    ILogger<DeleteRecurringJournalEntryHandler> logger,
    IRepository<RecurringJournalEntry> repository)
    : IRequestHandler<DeleteRecurringJournalEntryCommand>
{
    public async Task Handle(DeleteRecurringJournalEntryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var entry = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (entry == null)
            throw new RecurringJournalEntryNotFoundException(command.Id);

        await repository.DeleteAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Recurring journal entry deleted {EntryId}", command.Id);
    }
}
