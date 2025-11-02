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


        await repository.AddAsync(journalEntry, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new CreateJournalEntryResponse(journalEntry.Id);
    }
}
