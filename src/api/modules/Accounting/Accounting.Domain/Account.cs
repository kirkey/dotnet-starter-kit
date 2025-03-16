using Accounting.Domain.Events;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;
public class Account : AuditableEntity, IAggregateRoot
{
    public string AccountCategory { get; private set; }
    public string ParentCode { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    
    private Account() { }

    private Account(DefaultIdType id, string accountCategory, string type, string parentCode, string code, string name, decimal balance,
        string? description = null, string? notes = null)
    {
        Id = id;
        AccountCategory = accountCategory;
        Type = type;
        ParentCode = parentCode;
        Code = code;
        Name = name;
        Balance = balance;

        Description = description;
        Notes = notes;

        QueueDomainEvent(new AccountCreated(Id, AccountCategory, Type, Code, Name, Balance, Description, Notes));
        AccountMetrics.Created.Add(1);
    }

    public static Account Create(string accountCategory, string type, string parentCode, string code, string name, decimal balance = 0, string? description = null, string? notes = null)
    {
        return new Account(DefaultIdType.NewGuid(), accountCategory, type, parentCode, code, name, balance, description, notes);
    }

    public Account Update(string? accountCategory, string? type, string? parentCode, string? code, string? name, decimal balance, string? description, string? notes)
    {
        bool isUpdated = false;

        if (accountCategory is not null && AccountCategory != accountCategory)
        {
            AccountCategory = accountCategory;
            isUpdated = true;
        }
        
        if (type is not null && Type != type)
        {
            Type = type;
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
            
        if (AccountCategory is "Asset" or "Expense")
            Balance += amount;
        else
            Balance -= amount;
            
        QueueDomainEvent(new AccountBalanceChanged(Id, Balance, amount, "Debit"));
        return this;
    }

    public Account Credit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Credit amount must be positive", nameof(amount));
            
        if (AccountCategory is "Liability" or "Equity" or "Revenue")
            Balance += amount;
        else
            Balance -= amount;
            
        QueueDomainEvent(new AccountBalanceChanged(Id, Balance, amount, "Credit"));
        return this;
    }

    public Account Activate()
    {
        if (Status == "Active")
        {
            return this;
        }

        Status = "Active";
        QueueDomainEvent(new AccountStatusChanged(Id, Status));
        return this;
    }

    public Account Deactivate()
    {
        if (Status == "Inactive")
        {
            return this;
        }

        Status = "Inactive";
        QueueDomainEvent(new AccountStatusChanged(Id, Status));
        return this;
    }
}
