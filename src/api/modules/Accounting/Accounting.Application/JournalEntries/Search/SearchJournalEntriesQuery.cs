using Accounting.Application.JournalEntries.Responses;
using PaginationFilter = FSH.Framework.Core.Paging.PaginationFilter;

namespace Accounting.Application.JournalEntries.Search;

/// <summary>
/// Query used to search JournalEntries with pagination and common filters.
/// </summary>
public sealed class SearchJournalEntriesQuery : PaginationFilter, IRequest<PagedList<JournalEntryResponse>>
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
    
    /// <summary>
    /// Filter by approval status: Pending, Approved, or Rejected.
    /// </summary>
    public string? ApprovalStatus { get; set; }
    
    /// <summary>
    /// Filter by accounting period identifier.
    /// </summary>
    public DefaultIdType? PeriodId { get; set; }
}
