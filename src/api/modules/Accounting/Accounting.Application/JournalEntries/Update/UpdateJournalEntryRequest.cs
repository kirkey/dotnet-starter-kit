namespace Accounting.Application.JournalEntries.Update;

/// <summary>
/// Command to update a JournalEntry's metadata (not allowed if posted).
/// </summary>
public sealed record UpdateJournalEntryCommand(
    DefaultIdType Id,
    string? ReferenceNumber = null,
    DateTime? Date = null,
    string? Source = null,
    DefaultIdType? PeriodId = null,
    decimal? OriginalAmount = null,
    string? Description = null,
    string? Notes = null
) : IRequest<UpdateJournalEntryResponse>;
