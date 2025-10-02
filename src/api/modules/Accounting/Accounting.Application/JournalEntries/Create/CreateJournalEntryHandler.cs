using Accounting.Domain.Entities;

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

        // Add lines if provided
        if (request.Lines is { Count: > 0 })
        {
            foreach (var line in request.Lines)
            {
                journalEntry.AddLine(line.AccountId, line.DebitAmount, line.CreditAmount, line.Memo);
            }
        }

        await repository.AddAsync(journalEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateJournalEntryResponse(journalEntry.Id);
    }
}
