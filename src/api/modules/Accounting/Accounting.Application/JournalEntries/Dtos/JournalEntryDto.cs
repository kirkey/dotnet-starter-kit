namespace Accounting.Application.JournalEntries.Dtos;

public record JournalEntryDto(
    DefaultIdType Id,
    string Name,
    string Description,
    DateTime Date,
    string ReferenceNumber,
    string Source,
    bool IsPosted,
    DefaultIdType? PeriodId,
    DefaultIdType? CurrencyId,
    decimal ExchangeRate,
    decimal OriginalAmount);

public record JournalEntryLineDto(
    DefaultIdType Id,
    DefaultIdType JournalEntryId,
    DefaultIdType AccountId,
    decimal DebitAmount,
    decimal CreditAmount,
    string? Memo,
    string? Reference);
