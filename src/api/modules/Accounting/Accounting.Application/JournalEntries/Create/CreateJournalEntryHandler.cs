namespace Accounting.Application.JournalEntries.Create;

public sealed class CreateJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<CreateJournalEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var journalEntry = JournalEntry.Create(
            request.Date,
            request.ReferenceNumber,
            request.Description,
            request.Source,
            request.PeriodId,
            request.OriginalAmount);

        await repository.AddAsync(journalEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return journalEntry.Id;
    }
}
