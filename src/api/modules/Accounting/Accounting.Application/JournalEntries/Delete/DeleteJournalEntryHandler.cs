using Accounting.Domain;
using Accounting.Application.JournalEntries.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.JournalEntries.Delete;

public sealed class DeleteJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<DeleteJournalEntryRequest>
{
    public async Task Handle(DeleteJournalEntryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry == null) throw new JournalEntryNotFoundException(request.Id.ToString());

        // Check if already posted - cannot delete posted entries
        if (entry.IsPosted) throw new JournalEntryAlreadyPostedException(request.Id.ToString());

        await repository.DeleteAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
