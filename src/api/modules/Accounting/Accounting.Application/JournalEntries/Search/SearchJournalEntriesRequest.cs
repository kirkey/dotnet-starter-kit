using Accounting.Application.JournalEntries.Dtos;

namespace Accounting.Application.JournalEntries.Search;

public class SearchJournalEntriesQuery : PaginationFilter, IRequest<PagedList<JournalEntryDto>>
{
    public string? ReferenceNumber { get; set; }
    public string? Source { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
