using Accounting.Domain.Events.JournalEntry;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class JournalEntry : AuditableEntity, IAggregateRoot
{
    public DateTime Date { get; private set; }
    public string ReferenceNumber { get; private set; }
    public string Source { get; private set; }
    public bool IsPosted { get; private set; }
    public DefaultIdType? PeriodId { get; private set; }
    public DefaultIdType? CurrencyId { get; private set; }
    public decimal ExchangeRate { get; private set; }
    public decimal OriginalAmount { get; private set; }
    
    private readonly List<JournalEntryLine> _lines = new();
    public IReadOnlyCollection<JournalEntryLine> Lines => _lines.AsReadOnly();
    
    
    private JournalEntry()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private JournalEntry(DateTime date, string referenceNumber, string description, string source,
        DefaultIdType? periodId = null, DefaultIdType? currencyId = null, decimal exchangeRate = 1.0m, decimal originalAmount = 0)
    {
        Date = date;
        ReferenceNumber = referenceNumber.Trim();
        Description = description.Trim();
        Source = source.Trim();
        IsPosted = false;
        PeriodId = periodId;
        CurrencyId = currencyId;
        ExchangeRate = exchangeRate;
        OriginalAmount = originalAmount;

        QueueDomainEvent(new JournalEntryCreated(Id, Date, ReferenceNumber, Description, Source));
    }

    public static JournalEntry Create(DateTime date, string referenceNumber, string description, string source,
        DefaultIdType? periodId = null, DefaultIdType? currencyId = null, decimal exchangeRate = 1.0m, decimal originalAmount = 0)
    {
        return new JournalEntry(date, referenceNumber, description, source, periodId, currencyId, exchangeRate, originalAmount);
    }

    public JournalEntry Update(DateTime? date, string? referenceNumber, string? description, string? source,
        DefaultIdType? periodId, DefaultIdType? currencyId, decimal? exchangeRate, decimal? originalAmount)
    {
        bool isUpdated = false;

        if (IsPosted)
            throw new JournalEntryCannotBeModifiedException(Id);

        if (date.HasValue && Date != date.Value)
        {
            Date = date.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(referenceNumber) && ReferenceNumber != referenceNumber)
        {
            ReferenceNumber = referenceNumber.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description)
        {
            Description = description.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(source) && Source != source)
        {
            Source = source.Trim();
            isUpdated = true;
        }

        if (periodId != PeriodId)
        {
            PeriodId = periodId;
            isUpdated = true;
        }

        if (currencyId != CurrencyId)
        {
            CurrencyId = currencyId;
            isUpdated = true;
        }

        if (exchangeRate.HasValue && ExchangeRate != exchangeRate.Value)
        {
            ExchangeRate = exchangeRate.Value;
            isUpdated = true;
        }

        if (originalAmount.HasValue && OriginalAmount != originalAmount.Value)
        {
            OriginalAmount = originalAmount.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new JournalEntryUpdated(this));
        }

        return this;
    }

    public JournalEntry AddLine(DefaultIdType accountId, decimal debitAmount, decimal creditAmount, string? description = null)
    {
        if (IsPosted)
            throw new JournalEntryCannotBeModifiedException(Id);

        var line = JournalEntryLine.Create(Id, accountId, debitAmount, creditAmount, description);
        _lines.Add(line);
        
        QueueDomainEvent(new JournalEntryLineAdded(Id, line.Id, accountId, debitAmount, creditAmount));
        return this;
    }

    public JournalEntry Post()
    {
        if (IsPosted)
            throw new JournalEntryAlreadyPostedException(Id);

        if (!IsBalanced())
            throw new JournalEntryNotBalancedException(Id);

        IsPosted = true;
        QueueDomainEvent(new JournalEntryPosted(Id, Date));
        return this;
    }

    public JournalEntry Reverse(DateTime reversalDate, string reversalReason)
    {
        if (!IsPosted)
            throw new JournalEntryCannotBeModifiedException(Id);

        QueueDomainEvent(new JournalEntryReversed(Id, reversalDate, reversalReason));
        return this;
    }

    private bool IsBalanced()
    {
        var totalDebits = _lines.Sum(l => l.DebitAmount);
        var totalCredits = _lines.Sum(l => l.CreditAmount);
        return Math.Abs(totalDebits - totalCredits) < 0.01m;
    }
}

public class JournalEntryLine : BaseEntity
{
    public DefaultIdType JournalEntryId { get; private set; }
    public DefaultIdType AccountId { get; private set; }
    public decimal DebitAmount { get; private set; }
    public decimal CreditAmount { get; private set; }
    public string? Description { get; private set; }

    private JournalEntryLine(DefaultIdType journalEntryId, DefaultIdType accountId, 
        decimal debitAmount, decimal creditAmount, string? description = null)
    {
        JournalEntryId = journalEntryId;
        AccountId = accountId;
        DebitAmount = debitAmount;
        CreditAmount = creditAmount;
        Description = description?.Trim();
    }

    public static JournalEntryLine Create(DefaultIdType journalEntryId, DefaultIdType accountId,
        decimal debitAmount, decimal creditAmount, string? description = null)
    {
        if (debitAmount < 0 || creditAmount < 0)
            throw new InvalidJournalEntryLineAmountException();

        if (debitAmount > 0 && creditAmount > 0)
            throw new InvalidJournalEntryLineAmountException();

        if (debitAmount == 0 && creditAmount == 0)
            throw new InvalidJournalEntryLineAmountException();

        return new JournalEntryLine(journalEntryId, accountId, debitAmount, creditAmount, description);
    }

    public JournalEntryLine Update(decimal? debitAmount, decimal? creditAmount, string? description)
    {
        if (debitAmount.HasValue && DebitAmount != debitAmount.Value)
        {
            DebitAmount = debitAmount.Value;
        }

        if (creditAmount.HasValue && CreditAmount != creditAmount.Value)
        {
            CreditAmount = creditAmount.Value;
        }

        if (description != Description)
        {
            Description = description?.Trim();
        }

        return this;
    }
}
