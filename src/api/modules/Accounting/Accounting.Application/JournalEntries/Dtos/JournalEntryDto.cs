namespace Accounting.Application.JournalEntries.Dtos;

public class JournalEntryDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public string ReferenceNumber { get; set; } = null!;
    public string Source { get; set; } = null!;
    public bool IsPosted { get; set; }
    public DefaultIdType? PeriodId { get; set; }
    public DefaultIdType? CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal OriginalAmount { get; set; }

    public JournalEntryDto(
        DefaultIdType id,
        string name,
        string description,
        DateTime date,
        string referenceNumber,
        string source,
        bool isPosted,
        DefaultIdType? periodId,
        DefaultIdType? currencyId,
        decimal exchangeRate,
        decimal originalAmount)
    {
        Id = id;
        Name = name;
        Description = description;
        Date = date;
        ReferenceNumber = referenceNumber;
        Source = source;
        IsPosted = isPosted;
        PeriodId = periodId;
        CurrencyId = currencyId;
        ExchangeRate = exchangeRate;
        OriginalAmount = originalAmount;
    }
}

public class JournalEntryLineDto
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType JournalEntryId { get; set; }
    public DefaultIdType AccountId { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public string? Memo { get; set; }
    public string? Reference { get; set; }

    public JournalEntryLineDto(
        DefaultIdType id,
        DefaultIdType journalEntryId,
        DefaultIdType accountId,
        decimal debitAmount,
        decimal creditAmount,
        string? memo,
        string? reference)
    {
        Id = id;
        JournalEntryId = journalEntryId;
        AccountId = accountId;
        DebitAmount = debitAmount;
        CreditAmount = creditAmount;
        Memo = memo;
        Reference = reference;
    }
}
