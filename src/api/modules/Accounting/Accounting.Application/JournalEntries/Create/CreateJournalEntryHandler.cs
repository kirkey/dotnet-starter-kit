using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.JournalEntries.Create;

public sealed class CreateJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<CreateJournalEntryRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateJournalEntryRequest request, CancellationToken cancellationToken)
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
