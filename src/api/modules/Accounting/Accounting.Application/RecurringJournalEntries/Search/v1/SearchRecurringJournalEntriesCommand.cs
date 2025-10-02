using Accounting.Application.RecurringJournalEntries.Responses;

namespace Accounting.Application.RecurringJournalEntries.Search.v1;

public class SearchRecurringJournalEntriesCommand : PaginationFilter, IRequest<PagedList<RecurringJournalEntryResponse>>
{
    public string? TemplateCode { get; set; }
    public string? Frequency { get; set; }
    public string? Status { get; set; }
    public bool? IsActive { get; set; }
}
