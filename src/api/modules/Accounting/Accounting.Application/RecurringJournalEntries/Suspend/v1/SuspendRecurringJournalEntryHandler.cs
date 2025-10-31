namespace Accounting.Application.RecurringJournalEntries.Suspend.v1;

public sealed class SuspendRecurringJournalEntryHandler(
    ILogger<SuspendRecurringJournalEntryHandler> logger,
    IRepository<RecurringJournalEntry> repository)
    : IRequestHandler<SuspendRecurringJournalEntryCommand>
{
    public async Task Handle(SuspendRecurringJournalEntryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var entry = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (entry == null)
            throw new RecurringJournalEntryNotFoundException(command.Id);

        entry.Suspend(command.Reason);

        await repository.UpdateAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Recurring journal entry suspended {EntryId}", command.Id);
    }
}
