using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a line item in a journal entry.
/// </summary>
public sealed class JournalEntryLine : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int DescriptionMaxLength = 256;
    public const int ReferenceMaxLength = 128;
    
    public Guid JournalEntryId { get; private set; }
    public Guid AccountId { get; private set; }
    public int LineNumber { get; private set; }
    public string? Description { get; private set; }
    public decimal DebitAmount { get; private set; }
    public decimal CreditAmount { get; private set; }
    public string? Reference { get; private set; }
    public Guid? CostCenterId { get; private set; }
    public Guid? MemberId { get; private set; }
    public Guid? LoanId { get; private set; }
    public Guid? SavingsAccountId { get; private set; }

    private JournalEntryLine() { }

    public static JournalEntryLine CreateDebit(
        Guid journalEntryId,
        Guid accountId,
        int lineNumber,
        decimal amount,
        string? description = null,
        string? reference = null)
    {
        return new JournalEntryLine
        {
            JournalEntryId = journalEntryId,
            AccountId = accountId,
            LineNumber = lineNumber,
            DebitAmount = amount,
            CreditAmount = 0,
            Description = description,
            Reference = reference
        };
    }

    public static JournalEntryLine CreateCredit(
        Guid journalEntryId,
        Guid accountId,
        int lineNumber,
        decimal amount,
        string? description = null,
        string? reference = null)
    {
        return new JournalEntryLine
        {
            JournalEntryId = journalEntryId,
            AccountId = accountId,
            LineNumber = lineNumber,
            DebitAmount = 0,
            CreditAmount = amount,
            Description = description,
            Reference = reference
        };
    }

    public JournalEntryLine LinkToMember(Guid memberId)
    {
        MemberId = memberId;
        return this;
    }

    public JournalEntryLine LinkToLoan(Guid loanId)
    {
        LoanId = loanId;
        return this;
    }

    public JournalEntryLine LinkToSavings(Guid savingsAccountId)
    {
        SavingsAccountId = savingsAccountId;
        return this;
    }

    public JournalEntryLine SetCostCenter(Guid costCenterId)
    {
        CostCenterId = costCenterId;
        return this;
    }
}
