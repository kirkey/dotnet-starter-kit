using Accounting.Domain.Events.GeneralLedger;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class GeneralLedger : AuditableEntity, IAggregateRoot
{
    public DefaultIdType EntryId { get; private set; } // Foreign Key to Journal Entry
    public DefaultIdType AccountId { get; private set; } // Foreign Key to Chart of Accounts
    public decimal Debit { get; private set; }
    public decimal Credit { get; private set; }
    public string? Memo { get; private set; }
    public string UsoaClass { get; private set; } // Generation, Transmission, Distribution
    public DateTime TransactionDate { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public DefaultIdType? PeriodId { get; private set; }
    
    private GeneralLedger()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private GeneralLedger(DefaultIdType entryId, DefaultIdType accountId,
        decimal debit, decimal credit, string usoaClass, DateTime transactionDate,
        string? memo = null, string? referenceNumber = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        EntryId = entryId;
        AccountId = accountId;
        Debit = debit;
        Credit = credit;
        UsoaClass = usoaClass.Trim();
        TransactionDate = transactionDate;
        Memo = memo?.Trim();
        ReferenceNumber = referenceNumber?.Trim();
        PeriodId = periodId;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new GeneralLedgerEntryCreated(Id, EntryId, AccountId, Debit, Credit, UsoaClass, TransactionDate));
    }

    public static GeneralLedger Create(DefaultIdType entryId, DefaultIdType accountId,
        decimal debit, decimal credit, string usoaClass, DateTime transactionDate,
        string? memo = null, string? referenceNumber = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        if (debit < 0 || credit < 0)
            throw new InvalidGeneralLedgerAmountException("Debit and Credit amounts cannot be negative");

        if (debit > 0 && credit > 0)
            throw new InvalidGeneralLedgerAmountException("Both Debit and Credit cannot have amounts");

        if (debit == 0 && credit == 0)
            throw new InvalidGeneralLedgerAmountException("Either Debit or Credit must have an amount");

        if (!IsValidUsoaClass(usoaClass))
            throw new InvalidUsoaClassException($"Invalid USOA class: {usoaClass}");

        return new GeneralLedger(entryId, accountId, debit, credit, usoaClass,
            transactionDate, memo, referenceNumber, periodId, description, notes);
    }

    public GeneralLedger Update(decimal? debit = null, decimal? credit = null, string? memo = null,
        string? usoaClass = null, string? referenceNumber = null, string? description = null, string? notes = null)
    {
        bool isUpdated = false;

        if (debit.HasValue && Debit != debit.Value)
        {
            if (debit.Value < 0)
                throw new InvalidGeneralLedgerAmountException("Debit amount cannot be negative");
            Debit = debit.Value;
            isUpdated = true;
        }

        if (credit.HasValue && Credit != credit.Value)
        {
            if (credit.Value < 0)
                throw new InvalidGeneralLedgerAmountException("Credit amount cannot be negative");
            Credit = credit.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(memo) && Memo != memo.Trim())
        {
            Memo = memo.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(usoaClass) && UsoaClass != usoaClass.Trim())
        {
            if (!IsValidUsoaClass(usoaClass))
                throw new InvalidUsoaClassException($"Invalid USOA class: {usoaClass}");
            UsoaClass = usoaClass.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(referenceNumber) && ReferenceNumber != referenceNumber.Trim())
        {
            ReferenceNumber = referenceNumber.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description?.Trim())
        {
            Description = description.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(notes) && Notes != notes?.Trim())
        {
            Notes = notes.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new GeneralLedgerEntryUpdated(Id, EntryId, AccountId, Debit, Credit, UsoaClass));
        }

        return this;
    }

    private static bool IsValidUsoaClass(string usoaClass)
    {
        var validClasses = new[] { "Generation", "Transmission", "Distribution", "Customer Service", 
            "Sales", "Administrative", "General", "Maintenance" };
        return validClasses.Contains(usoaClass.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
