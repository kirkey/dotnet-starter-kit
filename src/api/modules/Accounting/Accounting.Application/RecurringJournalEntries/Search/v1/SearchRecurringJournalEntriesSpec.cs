using Accounting.Application.RecurringJournalEntries.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.RecurringJournalEntries.Search.v1;

/// <summary>
/// Specification for searching recurring journal entries with various filters and pagination support.
/// </summary>
public class SearchRecurringJournalEntriesSpec : EntitiesByPaginationFilterSpec<RecurringJournalEntry, RecurringJournalEntryResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchRecurringJournalEntriesSpec"/> class.
    /// </summary>
    /// <param name="request">The search recurring journal entries command containing filter criteria and pagination parameters.</param>
    public SearchRecurringJournalEntriesSpec(SearchRecurringJournalEntriesCommand request)
        : base(request)
    {
        Query
            .Where(e => e.TemplateCode.Contains(request.TemplateCode!), !string.IsNullOrEmpty(request.TemplateCode))
            .Where(e => e.Frequency.ToString() == request.Frequency, !string.IsNullOrEmpty(request.Frequency))
            .Where(e => e.Status.ToString() == request.Status, !string.IsNullOrEmpty(request.Status))
            .Where(e => e.IsActive == request.IsActive!.Value, request.IsActive.HasValue)
            .OrderByDescending(e => e.CreatedOn, !request.HasOrderBy());
    }
}
