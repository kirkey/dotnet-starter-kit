namespace Accounting.Application.RecurringJournalEntries.Update.v1;

public sealed class UpdateRecurringJournalEntryHandler(
    IRepository<RecurringJournalEntry> repository,
    ILogger<UpdateRecurringJournalEntryHandler> logger)
    : IRequestHandler<UpdateRecurringJournalEntryCommand, DefaultIdType>
{
    private readonly IRepository<RecurringJournalEntry> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateRecurringJournalEntryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateRecurringJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating recurring journal entry {Id}", request.Id);

        var entry = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry == null) throw new NotFoundException($"Recurring journal entry with ID {request.Id} not found");

        entry.Update(request.Description, request.Amount, request.EndDate, request.Memo, request.Notes);
        await _repository.UpdateAsync(entry, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Recurring journal entry {TemplateCode} updated successfully", entry.TemplateCode);
        return entry.Id;
    }
}

