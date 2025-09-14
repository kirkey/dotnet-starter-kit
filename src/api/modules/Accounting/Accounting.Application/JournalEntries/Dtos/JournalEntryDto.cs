namespace Accounting.Application.JournalEntries.Dtos;

public class JournalEntryDto : BaseDto
{
    public DateTime Date { get; set; }
    public string ReferenceNumber { get; set; } = null!;
    public string Source { get; set; } = null!;
    public bool IsPosted { get; set; }
    public DefaultIdType? PeriodId { get; set; }
    public decimal OriginalAmount { get; set; }
}

public class JournalEntryLineDto(
    DefaultIdType id,
    DefaultIdType journalEntryId,
    DefaultIdType accountId,
    decimal debitAmount,
    decimal creditAmount,
    string? memo,
    string? reference)
{
    public DefaultIdType Id { get; set; } = id;
    public DefaultIdType JournalEntryId { get; set; } = journalEntryId;
    public DefaultIdType AccountId { get; set; } = accountId;
    public decimal DebitAmount { get; set; } = debitAmount;
    public decimal CreditAmount { get; set; } = creditAmount;
    public string? Memo { get; set; } = memo;
    public string? Reference { get; set; } = reference;
}
