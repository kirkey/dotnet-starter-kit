using Accounting.Application.RecurringJournalEntries.Responses;

namespace Accounting.Application.RecurringJournalEntries.Search.v1;

/// <summary>
/// Request to search recurring journal entries with filters and pagination.
/// </summary>
public sealed class SearchRecurringJournalEntriesRequest : PaginationFilter, IRequest<PagedList<RecurringJournalEntryResponse>>
{
    public string? TemplateCode { get; init; }
    public string? Frequency { get; init; }
    public string? Status { get; init; }
    public bool? IsActive { get; init; }
}
