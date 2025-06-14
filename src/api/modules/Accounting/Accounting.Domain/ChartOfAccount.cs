using Accounting.Domain.Events;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;
public class ChartOfAccount : AuditableEntity, IAggregateRoot
{
    public string AccountCategory { get; private set; }
    public string AccountType { get; private set; }
    public string ParentCode { get; private set; }
    public string AccountCode { get; private set; }
    public decimal Balance { get; private set; }
    
    private ChartOfAccount(DefaultIdType id, string accountCategory, string accountType, string parentCode, string accountCode, string name, decimal balance,
        string? description = null, string? notes = null)
    {
        Id = id;
        AccountCategory = accountCategory.Trim();
        AccountType = accountType.Trim();
        ParentCode = parentCode.Trim();
        AccountCode = accountCode.Trim();
        Name = name.Trim();
        Balance = balance;

        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new ChartOfAccountCreated(Id, AccountCategory, AccountType, AccountCode, Name, Balance, Description, Notes));
        AccountMetrics.Created.Add(1);
    }

    public static ChartOfAccount Create(string accountCategory, string accountType, string parentCode, string accountCode, string name, decimal balance = 0, string? description = null, string? notes = null)
    {
        return new ChartOfAccount(DefaultIdType.NewGuid(), accountCategory, accountType, parentCode, accountCode, name, balance, description, notes);
    }

    public ChartOfAccount Update(string? accountCategory, string? accountType, string? parentCode, string? accountCode, string? name, decimal balance, string? description, string? notes)
    {
        bool isUpdated = false;

        if (accountCategory is not null && AccountCategory != accountCategory)
        {
            AccountCategory = accountCategory.Trim();
            isUpdated = true;
        }
        
        if (accountType is not null && AccountType != accountType)
        {
            AccountType = accountType.Trim();
            isUpdated = true;
        }
        
        if (!string.IsNullOrWhiteSpace(parentCode) && !string.Equals(ParentCode, parentCode, StringComparison.OrdinalIgnoreCase))
        {
            ParentCode = parentCode.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(accountCode) && !string.Equals(AccountCode, accountCode, StringComparison.OrdinalIgnoreCase))
        {
            AccountCode = accountCode.Trim();
            isUpdated = true;
        }
        
        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name.Trim();
            isUpdated = true;
        }
        
        if (balance != Balance)
        {
            Balance = balance;
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }
        
        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ChartOfAccountUpdated(this));
        }

        return this;
    }

    public ChartOfAccount Debit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Debit amount must be positive", nameof(amount));
            
        if (AccountCategory is "Asset" or "Expense")
            Balance += amount;
        else
            Balance -= amount;
            
        QueueDomainEvent(new ChartOfAccountBalanceChanged(Id, Balance, amount, "Debit"));
        return this;
    }

    public ChartOfAccount Credit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Credit amount must be positive", nameof(amount));
            
        if (AccountCategory is "Liability" or "Equity" or "Revenue")
            Balance += amount;
        else
            Balance -= amount;
            
        QueueDomainEvent(new ChartOfAccountBalanceChanged(Id, Balance, amount, "Credit"));
        return this;
    }

    public ChartOfAccount Activate()
    {
        if (Status == "Active")
        {
            return this;
        }

        Status = "Active";
        QueueDomainEvent(new ChartOfAccountStatusChanged(Id, Status));
        return this;
    }

    public ChartOfAccount Deactivate()
    {
        if (Status == "Inactive")
        {
            return this;
        }

        Status = "Inactive";
        QueueDomainEvent(new ChartOfAccountStatusChanged(Id, Status));
        return this;
    }
}
