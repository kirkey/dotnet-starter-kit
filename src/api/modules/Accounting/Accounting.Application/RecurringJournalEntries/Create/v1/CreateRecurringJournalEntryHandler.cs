namespace Accounting.Application.RecurringJournalEntries.Create.v1;

public sealed class CreateRecurringJournalEntryHandler(
    ILogger<CreateRecurringJournalEntryHandler> logger,
    IRepository<RecurringJournalEntry> repository)
    : IRequestHandler<CreateRecurringJournalEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateRecurringJournalEntryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (!Enum.TryParse<RecurrenceFrequency>(command.Frequency, true, out var frequency))
            throw new ArgumentException($"Invalid frequency: {command.Frequency}", nameof(command.Frequency));

        var entry = RecurringJournalEntry.Create(
            templateCode: command.TemplateCode,
            description: command.Description ?? string.Empty,
            frequency: frequency,
            amount: command.Amount,
            debitAccountId: command.DebitAccountId,
            creditAccountId: command.CreditAccountId,
            startDate: command.StartDate,
            endDate: command.EndDate,
            customIntervalDays: command.CustomIntervalDays,
            memo: command.Memo,
            notes: command.Notes);

        await repository.AddAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Recurring journal entry created {EntryId}", entry.Id);

        return entry.Id;
    }
}
