using Accounting.Domain;
using Accounting.Application.JournalEntries.Dtos;
using Accounting.Application.JournalEntries.Exceptions;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.JournalEntries.Get;

public sealed class GetJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IReadRepository<JournalEntry> repository,
    ICacheService cache)
    : IRequestHandler<GetJournalEntryRequest, JournalEntryDto>
{
    public async Task<JournalEntryDto> Handle(GetJournalEntryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"journal:{request.Id}",
            async () =>
            {
                var entry = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (entry == null) throw new JournalEntryNotFoundException(request.Id);
                return new JournalEntryDto(
                    entry.Id,
                    entry.Name!,
                    entry.Description!,
                    entry.Date,
                    entry.ReferenceNumber,
                    entry.Source,
                    entry.IsPosted,
                    entry.PeriodId,
                    entry.CurrencyId,
                    entry.ExchangeRate,
                    entry.OriginalAmount);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
