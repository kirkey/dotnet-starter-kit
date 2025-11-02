using Accounting.Application.JournalEntries.Lines.Responses;

namespace Accounting.Application.JournalEntries.Lines.Get;

/// <summary>
/// Query to get a single journal entry line by ID.
/// </summary>
/// <param name="Id">The journal entry line identifier.</param>
public sealed record GetJournalEntryLineQuery(DefaultIdType Id) : IRequest<JournalEntryLineResponse>;
