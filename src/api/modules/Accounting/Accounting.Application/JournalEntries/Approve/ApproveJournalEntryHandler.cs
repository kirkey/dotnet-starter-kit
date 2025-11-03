namespace Accounting.Application.JournalEntries.Approve;

/// <summary>
/// Handler for approving a journal entry.
/// </summary>
public sealed class ApproveJournalEntryHandler(
    ILogger<ApproveJournalEntryHandler> logger,
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<ApproveJournalEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApproveJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Use spec to load entry with lines for balance validation
        var spec = new Specs.GetJournalEntryWithLinesSpec(request.JournalEntryId);
        var journalEntry = await ((IReadRepository<JournalEntry>)repository).FirstOrDefaultAsync(spec, cancellationToken);
        
        if (journalEntry == null)
        {
            throw new JournalEntryNotFoundException(request.JournalEntryId);
        }

        if (string.IsNullOrWhiteSpace(request.ApprovedBy))
        {
            throw new ArgumentException("ApprovedBy is required for journal entry approval.");
        }

        // Validate that the entry is balanced before approving
        journalEntry.ValidateBalance();

        journalEntry.Approve(request.ApprovedBy);

        await repository.UpdateAsync(journalEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Journal entry {JournalEntryId} approved by {ApprovedBy}", 
            request.JournalEntryId, request.ApprovedBy);

        return journalEntry.Id;
    }
}
