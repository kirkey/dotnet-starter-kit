namespace Accounting.Application.RecurringJournalEntries.Approve.v1;

public sealed class ApproveRecurringJournalEntryHandler(
    ILogger<ApproveRecurringJournalEntryHandler> logger,
    IRepository<RecurringJournalEntry> repository)
    : IRequestHandler<ApproveRecurringJournalEntryCommand>
{
    public async Task Handle(ApproveRecurringJournalEntryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var entry = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (entry == null)
            throw new RecurringJournalEntryNotFoundException(command.Id);

        entry.Approve(command.ApprovedBy);

        await repository.UpdateAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Recurring journal entry approved {EntryId}", command.Id);
    }
}
