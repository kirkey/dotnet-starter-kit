using Accounting.Application.JournalEntries.Dtos;

namespace Accounting.Application.JournalEntries.Search;

/// <summary>
/// Query used to search JournalEntries with pagination and common filters.
/// </summary>
public sealed class SearchJournalEntriesQuery : PaginationFilter, IRequest<PagedList<JournalEntryDto>>
{
    /// <summary>
    /// Filter by external reference number (partial match).
    /// </summary>
    public string? ReferenceNumber { get; set; }

    /// <summary>
    /// Filter by source system/module (partial match).
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Start of date range (inclusive).
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// End of date range (inclusive).
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Filter by posted state. When null, do not filter by posted state.
    /// </summary>
    public bool? IsPosted { get; set; }
}
