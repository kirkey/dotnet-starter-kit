using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a chart of accounts for MFI accounting.
/// </summary>
public sealed class ChartOfAccount : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int AccountCodeMaxLength = 32;
    public const int TypeMaxLength = 32;
    public const int CategoryMaxLength = 64;
    public const int StatusMaxLength = 32;
    
    // Account Types
    public const string TypeAsset = "Asset";
    public const string TypeLiability = "Liability";
    public const string TypeEquity = "Equity";
    public const string TypeIncome = "Income";
    public const string TypeExpense = "Expense";
    
    // Account Status
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusClosed = "Closed";

    public string AccountCode { get; private set; } = default!;
    public string AccountType { get; private set; } = default!;
    public string? Category { get; private set; }
    public string Status { get; private set; } = StatusActive;
    public Guid? ParentAccountId { get; private set; }
    public int Level { get; private set; }
    public bool IsHeaderAccount { get; private set; }
    public bool AllowPosting { get; private set; }
    public decimal Balance { get; private set; }
    public decimal DebitBalance { get; private set; }
    public decimal CreditBalance { get; private set; }
    public bool IsControlAccount { get; private set; }
    public string? ControlledEntity { get; private set; }
    public Guid? BranchId { get; private set; }
    public bool RequiresBankReconciliation { get; private set; }
    public string? BankAccountNumber { get; private set; }
    public int DisplayOrder { get; private set; }

    private ChartOfAccount() { }

    public static ChartOfAccount Create(
        string accountCode,
        string name,
        string accountType,
        int level = 1,
        bool isHeaderAccount = false,
        bool allowPosting = true,
        string? category = null,
        string? description = null,
        Guid? parentAccountId = null)
    {
        var account = new ChartOfAccount
        {
            AccountCode = accountCode,
            AccountType = accountType,
            Level = level,
            IsHeaderAccount = isHeaderAccount,
            AllowPosting = !isHeaderAccount && allowPosting,
            Category = category,
            ParentAccountId = parentAccountId,
            Status = StatusActive,
            Balance = 0,
            DebitBalance = 0,
            CreditBalance = 0
        };
        account.Name = name;
        account.Description = description;

        account.QueueDomainEvent(new ChartOfAccountCreated(account));
        return account;
    }

    public ChartOfAccount Debit(decimal amount)
    {
        DebitBalance += amount;
        UpdateBalance();
        return this;
    }

    public ChartOfAccount Credit(decimal amount)
    {
        CreditBalance += amount;
        UpdateBalance();
        return this;
    }

    private void UpdateBalance()
    {
        // Assets and Expenses are debit-normal
        // Liabilities, Equity, and Income are credit-normal
        Balance = AccountType switch
        {
            TypeAsset or TypeExpense => DebitBalance - CreditBalance,
            _ => CreditBalance - DebitBalance
        };
    }

    public ChartOfAccount SetAsControlAccount(string controlledEntity)
    {
        IsControlAccount = true;
        ControlledEntity = controlledEntity;
        return this;
    }

    public ChartOfAccount SetBankDetails(string bankAccountNumber)
    {
        BankAccountNumber = bankAccountNumber;
        RequiresBankReconciliation = true;
        return this;
    }

    public ChartOfAccount Activate()
    {
        Status = StatusActive;
        return this;
    }

    public ChartOfAccount Deactivate()
    {
        Status = StatusInactive;
        return this;
    }

    public ChartOfAccount Close()
    {
        if (Balance != 0)
            throw new InvalidOperationException("Cannot close account with non-zero balance.");
        
        Status = StatusClosed;
        AllowPosting = false;
        return this;
    }

    public ChartOfAccount Update(
        string? name = null,
        string? category = null,
        string? description = null,
        int? displayOrder = null)
    {
        if (name is not null) Name = name;
        if (category is not null) Category = category;
        if (description is not null) Description = description;
        if (displayOrder.HasValue) DisplayOrder = displayOrder.Value;

        QueueDomainEvent(new ChartOfAccountUpdated(this));
        return this;
    }
}
