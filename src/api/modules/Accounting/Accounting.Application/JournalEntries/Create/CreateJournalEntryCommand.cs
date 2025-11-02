namespace Accounting.Application.JournalEntries.Create;

/// <summary>
/// Command to create a new Journal Entry.
/// </summary>
public sealed record CreateJournalEntryCommand(
    DateTime Date,
    string ReferenceNumber,
    string Source,
    string Description,
    IReadOnlyCollection<JournalEntryLineDto>? Lines = null,
    DefaultIdType? PeriodId = null,
    decimal OriginalAmount = 0,
    string? Notes = null
) : IRequest<CreateJournalEntryResponse>;

/// <summary>
/// DTO for journal entry line in create/update commands.
/// </summary>
public sealed record JournalEntryLineDto(
    DefaultIdType AccountId,
    decimal DebitAmount,
    decimal CreditAmount,
    string? Description = null
);
