using Accounting.Application.RecurringJournalEntries.Responses;

namespace Accounting.Application.RecurringJournalEntries.Get.v1;

public class GetRecurringJournalEntryRequest(DefaultIdType id) : IRequest<RecurringJournalEntryResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
