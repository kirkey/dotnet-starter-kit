namespace Accounting.Application.RecurringJournalEntries.Reactivate.v1;

public sealed class ReactivateRecurringJournalEntryHandler(
    ILogger<ReactivateRecurringJournalEntryHandler> logger,
    IRepository<RecurringJournalEntry> repository)
    : IRequestHandler<ReactivateRecurringJournalEntryCommand>
{
    public async Task Handle(ReactivateRecurringJournalEntryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var entry = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (entry == null)
            throw new RecurringJournalEntryNotFoundException(command.Id);

        entry.Reactivate();

        await repository.UpdateAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Recurring journal entry reactivated {EntryId}", command.Id);
    }
}
