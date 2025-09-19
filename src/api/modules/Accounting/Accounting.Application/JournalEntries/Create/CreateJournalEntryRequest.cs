using Accounting.Application.JournalEntries.Dtos;

namespace Accounting.Application.JournalEntries.Create;

public record CreateJournalEntryCommand(
    DefaultIdType? Id,
    DateTime Date,
    string ReferenceNumber,
    string Source,
    string Description,
    List<JournalEntryLineDto> Lines,
    string? Notes = null) : IRequest<DefaultIdType>;
