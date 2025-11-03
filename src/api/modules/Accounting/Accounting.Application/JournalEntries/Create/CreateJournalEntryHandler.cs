namespace Accounting.Application.JournalEntries.Create;

public sealed class CreateJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<CreateJournalEntryCommand, CreateJournalEntryResponse>
{
    public async Task<CreateJournalEntryResponse> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var journalEntry = JournalEntry.Create(
            request.Date,
            request.ReferenceNumber,
            request.Description,
            request.Source,
            request.PeriodId,
            request.OriginalAmount);

        // Add lines to the journal entry (validation ensures Lines is not null and has at least 2 items)
        if (request.Lines != null)
        {
            foreach (var lineDto in request.Lines)
            {
                journalEntry.AddLine(
                    lineDto.AccountId,
                    lineDto.DebitAmount,
                    lineDto.CreditAmount,
                    lineDto.Description,
                    lineDto.Reference);
            }
        }

        await repository.AddAsync(journalEntry, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new CreateJournalEntryResponse(journalEntry.Id);
    }
}
