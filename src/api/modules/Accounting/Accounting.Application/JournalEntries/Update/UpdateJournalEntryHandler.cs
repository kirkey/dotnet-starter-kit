using Accounting.Domain;
using Accounting.Application.JournalEntries.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.JournalEntries.Update;

public sealed class UpdateJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<UpdateJournalEntryRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateJournalEntryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry == null) throw new JournalEntryNotFoundException(request.Id);

        // Check if already posted
        if (entry.IsPosted) throw new JournalEntryAlreadyPostedException(request.Id);

        // Check for duplicate reference number (excluding current entry)
        if (!string.IsNullOrEmpty(request.ReferenceNumber) && request.ReferenceNumber != entry.ReferenceNumber)
        {
            // var existingByRef = await repository.FirstOrDefaultAsync(
            //     x => x.ReferenceNumber == request.ReferenceNumber && x.Id != request.Id, cancellationToken);
            // if (existingByRef != null)
            // {
            //     throw new JournalEntryReferenceNumberAlreadyExistsException(request.ReferenceNumber);
            // }
        }

        entry.Update(request.Date, request.ReferenceNumber, request.Description, request.Source,
                    request.PeriodId, request.CurrencyId, request.ExchangeRate,
                    request.OriginalAmount);

        await repository.UpdateAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entry.Id;
    }
}
