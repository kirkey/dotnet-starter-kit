using Accounting.Application.JournalEntries.Responses;

namespace Accounting.Application.JournalEntries.Create;

/// <summary>
/// Command to create a new Journal Entry.
/// </summary>
public sealed record CreateJournalEntryCommand(
    DefaultIdType? Id,
    DateTime Date,
    string ReferenceNumber,
    string Source,
    string Description,
    IReadOnlyCollection<JournalEntryLineResponse>? Lines = null,
    DefaultIdType? PeriodId = null,
    decimal OriginalAmount = 0,
    string? Notes = null
) : IRequest<CreateJournalEntryResponse>;
