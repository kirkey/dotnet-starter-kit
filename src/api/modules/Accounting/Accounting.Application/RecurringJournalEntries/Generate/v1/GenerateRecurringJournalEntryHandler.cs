namespace Accounting.Application.RecurringJournalEntries.Generate.v1;

public sealed class GenerateRecurringJournalEntryHandler(
    IRepository<RecurringJournalEntry> recurringRepository,
    IRepository<JournalEntry> journalRepository,
    ILogger<GenerateRecurringJournalEntryHandler> logger)
    : IRequestHandler<GenerateRecurringJournalEntryCommand, DefaultIdType>
{
    private readonly IRepository<RecurringJournalEntry> _recurringRepository = recurringRepository ?? throw new ArgumentNullException(nameof(recurringRepository));
    private readonly IRepository<JournalEntry> _journalRepository = journalRepository ?? throw new ArgumentNullException(nameof(journalRepository));
    private readonly ILogger<GenerateRecurringJournalEntryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(GenerateRecurringJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Generating journal entry from recurring template {Id}", request.Id);

        var template = await _recurringRepository.GetByIdAsync(request.Id, cancellationToken);
        if (template == null) throw new NotFoundException($"Recurring journal entry template with ID {request.Id} not found");

        var generateDate = request.GenerateForDate ?? DateTime.UtcNow;
        
        // Generate journal entry from template
        var journalNumber = $"REC-{template.TemplateCode}-{generateDate:yyyyMMdd}";
        var journalEntry = JournalEntry.Create(
            generateDate,
            journalNumber,
            template.Description ?? "Recurring journal entry",
            "RecurringTemplate",
            template.PostingBatchId,
            template.Amount
        );

        // Add debit line
        journalEntry.AddLine(
            template.DebitAccountId,
            template.Amount,
            0,
            template.Memo ?? $"Recurring: {template.Description}"
        );

        // Add credit line
        journalEntry.AddLine(
            template.CreditAccountId,
            0,
            template.Amount,
            template.Memo ?? $"Recurring: {template.Description}"
        );

        await _journalRepository.AddAsync(journalEntry, cancellationToken);
        
        // Record generation on template
        template.RecordGeneration(journalEntry.Id, generateDate);
        await _recurringRepository.UpdateAsync(template, cancellationToken);
        
        await _journalRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Journal entry {JournalNumber} generated from template {TemplateCode}", 
            journalNumber, template.TemplateCode);
        
        return journalEntry.Id;
    }
}
