using Accounting.Application.RecurringJournalEntries.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.RecurringJournalEntries.Search.v1;

public class SearchRecurringJournalEntriesSpec : Specification<RecurringJournalEntry, RecurringJournalEntryResponse>
{
    public SearchRecurringJournalEntriesSpec(SearchRecurringJournalEntriesCommand request)
    {
        Query
            .Where(e => string.IsNullOrEmpty(request.TemplateCode) || e.TemplateCode.Contains(request.TemplateCode))
            .Where(e => string.IsNullOrEmpty(request.Frequency) || e.Frequency.ToString() == request.Frequency)
            .Where(e => string.IsNullOrEmpty(request.Status) || e.Status.ToString() == request.Status)
            .Where(e => !request.IsActive.HasValue || e.IsActive == request.IsActive.Value)
            .OrderByDescending(e => e.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}
