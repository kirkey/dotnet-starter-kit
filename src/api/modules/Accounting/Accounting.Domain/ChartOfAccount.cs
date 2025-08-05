using Accounting.Domain.Events.ChartOfAccount;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;
public class ChartOfAccount : AuditableEntity, IAggregateRoot
{
    public string AccountCode { get; private set; } // USOA Account ID (e.g., 101, 403)
    public string AccountName { get; private set; } // Official USOA account name
    public string AccountType { get; private set; } // Asset, Liability, Equity, Revenue, Expense
    public DefaultIdType? SubAccountOf { get; private set; } // Hierarchical structure
    public string UsoaCategory { get; private set; } // Production, Transmission, Distribution
    public bool IsActive { get; private set; }
    
    // Additional fields for enhanced functionality
    public string ParentCode { get; private set; }
    public decimal Balance { get; private set; }
    public bool IsControlAccount { get; private set; }
    public string NormalBalance { get; private set; } // "Debit" or "Credit"
    public int AccountLevel { get; private set; }
    public bool AllowDirectPosting { get; private set; }
    public DefaultIdType? CurrencyId { get; private set; }
    public bool IsUsoaCompliant { get; private set; }
    public string? RegulatoryClassification { get; private set; } // FERC classification
    
    private ChartOfAccount(string accountCode, string accountName, string accountType, 
        string usoaCategory, DefaultIdType? subAccountOf = null, string? parentCode = null,
        decimal balance = 0, bool isControlAccount = false, string normalBalance = "Debit",
        bool isUsoaCompliant = true, string? regulatoryClassification = null,
        string? description = null, string? notes = null, DefaultIdType? currencyId = null)
    {
        AccountCode = accountCode.Trim();
        AccountName = accountName.Trim();
        Name = accountName.Trim(); // Keep for compatibility
        AccountType = accountType.Trim();
        SubAccountOf = subAccountOf;
        UsoaCategory = usoaCategory.Trim();
        IsActive = true;
        ParentCode = parentCode?.Trim() ?? string.Empty;
        Balance = balance;
        IsControlAccount = isControlAccount;
        NormalBalance = normalBalance.Trim();
        AccountLevel = CalculateAccountLevel(parentCode);
        AllowDirectPosting = !isControlAccount;
        CurrencyId = currencyId;
        IsUsoaCompliant = isUsoaCompliant;
        RegulatoryClassification = regulatoryClassification?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new ChartOfAccountCreated(Id, AccountCode, AccountName, AccountType, UsoaCategory, Description, Notes));
    }

    public static ChartOfAccount Create(string accountId, string accountName, string accountType, 
        string usoaCategory, DefaultIdType? subAccountOf = null, string? parentCode = null,
        decimal balance = 0, bool isControlAccount = false, string normalBalance = "Debit",
        bool isUsoaCompliant = true, string? regulatoryClassification = null,
        string? description = null, string? notes = null, DefaultIdType? currencyId = null)
    {
        if (string.IsNullOrWhiteSpace(accountId))
            throw new ChartOfAccountInvalidException("Account ID cannot be empty");
            
        if (string.IsNullOrWhiteSpace(accountName))
            throw new ChartOfAccountInvalidException("Account Name cannot be empty");
            
        if (!IsValidAccountType(accountType))
            throw new ChartOfAccountInvalidException($"Invalid account type: {accountType}");
            
        if (!IsValidUsoaCategory(usoaCategory))
            throw new ChartOfAccountInvalidException($"Invalid USOA category: {usoaCategory}");

        return new ChartOfAccount(accountId, accountName, accountType, usoaCategory,
            subAccountOf, parentCode, balance, isControlAccount, normalBalance, isUsoaCompliant, 
            regulatoryClassification, description, notes, currencyId);
    }

    public ChartOfAccount Update(string? accountName = null, string? accountType = null, 
        string? usoaCategory = null, DefaultIdType? subAccountOf = null, string? parentCode = null,
        bool? isControlAccount = null, string? normalBalance = null, bool? isUsoaCompliant = null,
        string? regulatoryClassification = null, string? description = null, string? notes = null)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(accountName) && AccountName != accountName.Trim())
        {
            Name = accountName.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(accountType) && AccountType != accountType.Trim())
        {
            if (!IsValidAccountType(accountType))
                throw new ChartOfAccountInvalidException($"Invalid account type: {accountType}");
            AccountType = accountType.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(usoaCategory) && UsoaCategory != usoaCategory.Trim())
        {
            if (!IsValidUsoaCategory(usoaCategory))
                throw new ChartOfAccountInvalidException($"Invalid USOA category: {usoaCategory}");
            UsoaCategory = usoaCategory.Trim();
            isUpdated = true;
        }

        if (subAccountOf != SubAccountOf)
        {
            SubAccountOf = subAccountOf;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(parentCode) && ParentCode != parentCode.Trim())
        {
            ParentCode = parentCode.Trim();
            AccountLevel = CalculateAccountLevel(ParentCode);
            isUpdated = true;
        }

        if (isControlAccount.HasValue && IsControlAccount != isControlAccount.Value)
        {
            IsControlAccount = isControlAccount.Value;
            AllowDirectPosting = !IsControlAccount;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(normalBalance) && NormalBalance != normalBalance.Trim())
        {
            NormalBalance = normalBalance.Trim();
            isUpdated = true;
        }

        if (isUsoaCompliant.HasValue && IsUsoaCompliant != isUsoaCompliant.Value)
        {
            IsUsoaCompliant = isUsoaCompliant.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(regulatoryClassification) && RegulatoryClassification != regulatoryClassification.Trim())
        {
            RegulatoryClassification = regulatoryClassification.Trim();
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
            QueueDomainEvent(new ChartOfAccountUpdated(Id, AccountCode, AccountName, AccountType, UsoaCategory, Description, Notes));
        }

        return this;
    }

    public ChartOfAccount UpdateBalance(decimal newBalance)
    {
        if (Balance != newBalance)
        {
            Balance = newBalance;
            QueueDomainEvent(new ChartOfAccountBalanceUpdated(Id, AccountCode, Balance));
        }
        return this;
    }

    public ChartOfAccount Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new ChartOfAccountStatusChanged(Id, AccountCode, IsActive));
        }
        return this;
    }

    public ChartOfAccount Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new ChartOfAccountStatusChanged(Id, AccountCode, IsActive));
        }
        return this;
    }

    private static bool IsValidAccountType(string accountType)
    {
        var validTypes = new[] { "Asset", "Liability", "Equity", "Revenue", "Expense" };
        return validTypes.Contains(accountType.Trim(), StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsValidUsoaCategory(string usoaCategory)
    {
        var validCategories = new[] { "Production", "Transmission", "Distribution", "Customer Accounts", 
            "Customer Service", "Sales", "Administrative", "General", "Maintenance", "Operation" };
        return validCategories.Contains(usoaCategory.Trim(), StringComparer.OrdinalIgnoreCase);
    }

    private static int CalculateAccountLevel(string? parentCode)
    {
        if (string.IsNullOrWhiteSpace(parentCode))
            return 1;
        
        return parentCode.Split('.').Length + 1;
    }
}
