namespace Accounting.Application.JournalEntries.Create;

/// <summary>
/// Command to create a new Journal Entry.
/// </summary>
/// <param name="Date">Effective date of the journal entry.</param>
/// <param name="ReferenceNumber">External reference or document number.</param>
/// <param name="Source">Source system or module that created the entry.</param>
/// <param name="Description">Description of the journal entry.</param>
/// <param name="PeriodId">Optional accounting period identifier.</param>
/// <param name="OriginalAmount">Original amount for reference purposes.</param>
/// <param name="Notes">Optional notes.</param>
public sealed record CreateJournalEntryCommand(
    DateTime Date,
    string ReferenceNumber,
    string Source,
    string Description,
    DefaultIdType? PeriodId = null,
    decimal OriginalAmount = 0,
    string? Notes = null
) : IRequest<CreateJournalEntryResponse>;

