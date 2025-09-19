using JournalEntryAlreadyPostedException = Accounting.Application.JournalEntries.Exceptions.JournalEntryAlreadyPostedException;
using JournalEntryNotFoundException = Accounting.Application.JournalEntries.Exceptions.JournalEntryNotFoundException;

namespace Accounting.Application.JournalEntries.Update;

public sealed class UpdateJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<UpdateJournalEntryCommand, UpdateJournalEntryResponse>
{
    public async Task<UpdateJournalEntryResponse> Handle(UpdateJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry == null) throw new JournalEntryNotFoundException(request.Id.ToString());

        // Check if already posted
        if (entry.IsPosted) throw new JournalEntryAlreadyPostedException(request.Id.ToString());

        // Check for duplicate reference number (excluding current entry)
        if (!string.IsNullOrEmpty(request.ReferenceNumber) && request.ReferenceNumber != entry.ReferenceNumber)
        {
            // duplicate check could go here
        }

        entry.Update(request.Date, request.ReferenceNumber, request.Description, request.Source,
                    request.PeriodId, request.OriginalAmount);

        await repository.UpdateAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new UpdateJournalEntryResponse(entry.Id);
    }
}
