using Accounting.Domain.Enums;
using Accounting.Domain.Events;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;
public class Account : AuditableEntity, IAggregateRoot
{
    public Category Category { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public string ParentCode { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public new AccountStatus Status { get; private set; }
    
    private Account() { }

    private Account(DefaultIdType id, Category category, TransactionType transactionType, string parentCode, string code, string name, decimal balance,
        string? description = null, string? notes = null)
    {
        Id = id;
        Category = category;
        TransactionType = transactionType;
        ParentCode = parentCode;
        Code = code;
        Name = name;
        Balance = balance;

        Description = description;
        Notes = notes;

        Status = AccountStatus.Active;

        QueueDomainEvent(new AccountCreated(Id, Category, Code, Name, Balance, Description, Notes));
        AccountMetrics.Created.Add(1);
    }

    public static Account Create(Category category, TransactionType transactionType, string parentCode, string code, string name, decimal balance = 0, string? description = null, string? notes = null)
    {
        return new Account(DefaultIdType.NewGuid(), category, transactionType, parentCode, code, name, balance, description, notes);
    }

    public Account Update(Category? category, TransactionType? transactionType, string? parentCode, string? code, string? name, decimal balance, string? description, string? notes)
    {
        bool isUpdated = false;

        if (category.HasValue && Category != category.Value)
        {
            Category = category.Value;
            isUpdated = true;
        }
        
        if (transactionType.HasValue && TransactionType != transactionType.Value)
        {
            TransactionType = transactionType.Value;
            isUpdated = true;
        }
        
        if (!string.IsNullOrWhiteSpace(parentCode) && !string.Equals(ParentCode, parentCode, StringComparison.OrdinalIgnoreCase))
        {
            ParentCode = parentCode;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(code) && !string.Equals(Code, code, StringComparison.OrdinalIgnoreCase))
        {
            Code = code;
            isUpdated = true;
        }
        
        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }
        
        if (balance != Balance)
        {
            Balance = balance;
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description;
            isUpdated = true;
        }
        
        if (notes != Notes)
        {
            Notes = notes;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new AccountUpdated(this));
        }

        return this;
    }

    public Account Debit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Debit amount must be positive", nameof(amount));
            
        if (Category is Category.Asset or Category.Expense)
            Balance += amount;
        else
            Balance -= amount;
            
        QueueDomainEvent(new AccountBalanceChanged(Id, Balance, amount, TransactionType.Debit));
        return this;
    }

    public Account Credit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Credit amount must be positive", nameof(amount));
            
        if (Category is Category.Liability or Category.Equity or Category.Revenue)
            Balance += amount;
        else
            Balance -= amount;
            
        QueueDomainEvent(new AccountBalanceChanged(Id, Balance, amount, TransactionType.Credit));
        return this;
    }

    public Account Activate()
    {
        if (Status == AccountStatus.Active)
        {
            return this;
        }

        Status = AccountStatus.Active;
        QueueDomainEvent(new AccountStatusChanged(Id, Status));
        return this;
    }

    public Account Deactivate()
    {
        if (Status == AccountStatus.Inactive)
        {
            return this;
        }

        Status = AccountStatus.Inactive;
        QueueDomainEvent(new AccountStatusChanged(Id, Status));
        return this;
    }
}
