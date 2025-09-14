using Accounting.Domain.Events.ChartOfAccount;

namespace Accounting.Domain;
public class ChartOfAccount : AuditableEntity, IAggregateRoot
{
    private const int MaxAccountCodeLength = 16;
    private const int MaxAccountNameLength = 1024;
    private const int MaxAccountTypeLength = 32;
    private const int MaxUsoaCategoryLength = 16;
    private const int MaxParentCodeLength = 16;
    private const int MaxNormalBalanceLength = 8;
    private const int MaxRegulatoryClassificationLength = 256;
    private const int MaxDescriptionLength = 2048;
    private const int MaxNotesLength = 2048;

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
    public bool IsUsoaCompliant { get; private set; }
    public string? RegulatoryClassification { get; private set; } // FERC classification

    // Parameterless constructor for EF Core
    private ChartOfAccount()
    {
    }
    
    private ChartOfAccount(string accountCode, string accountName, string accountType, 
        string usoaCategory, DefaultIdType? subAccountOf = null, string? parentCode = null,
        decimal balance = 0, bool isControlAccount = false, string normalBalance = "Debit",
        bool isUsoaCompliant = true, string? regulatoryClassification = null,
        string? description = null, string? notes = null)
    {
        var ac = (accountCode ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(ac))
            throw new ChartOfAccountInvalidException("Account ID cannot be empty");
        if (ac.Length > MaxAccountCodeLength)
            throw new ChartOfAccountInvalidException($"Account code cannot exceed {MaxAccountCodeLength} characters.");

        var an = (accountName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(an))
            throw new ChartOfAccountInvalidException("Account Name cannot be empty");
        if (an.Length > MaxAccountNameLength)
            throw new ChartOfAccountInvalidException($"Account name cannot exceed {MaxAccountNameLength} characters.");

        var at = (accountType ?? string.Empty).Trim();
        if (!IsValidAccountType(at))
            throw new ChartOfAccountInvalidException($"Invalid account type: {accountType}");
        if (at.Length > MaxAccountTypeLength)
            throw new ChartOfAccountInvalidException($"Account type cannot exceed {MaxAccountTypeLength} characters.");

        var uc = (usoaCategory ?? string.Empty).Trim();
        if (!IsValidUsoaCategory(uc))
            throw new ChartOfAccountInvalidException($"Invalid USOA category: {usoaCategory}");
        if (uc.Length > MaxUsoaCategoryLength)
            throw new ChartOfAccountInvalidException($"USOA category cannot exceed {MaxUsoaCategoryLength} characters.");

        AccountCode = ac;
        AccountName = an;
        Name = an; // Keep for compatibility
        AccountType = at;
        SubAccountOf = subAccountOf;
        UsoaCategory = uc;
        IsActive = true;
        ParentCode = (parentCode ?? string.Empty).Trim();
        if (ParentCode.Length > MaxParentCodeLength)
            ParentCode = ParentCode.Substring(0, MaxParentCodeLength);

        Balance = balance;
        IsControlAccount = isControlAccount;
        NormalBalance = (normalBalance ?? "Debit").Trim();
        if (NormalBalance.Length > MaxNormalBalanceLength)
            NormalBalance = NormalBalance.Substring(0, MaxNormalBalanceLength);

        AccountLevel = CalculateAccountLevel(ParentCode);
        AllowDirectPosting = !isControlAccount;
        IsUsoaCompliant = isUsoaCompliant;
        RegulatoryClassification = regulatoryClassification?.Trim();
        if (RegulatoryClassification?.Length > MaxRegulatoryClassificationLength)
            RegulatoryClassification = RegulatoryClassification.Substring(0, MaxRegulatoryClassificationLength);

        Description = description?.Trim();
        if (Description?.Length > MaxDescriptionLength)
            Description = Description.Substring(0, MaxDescriptionLength);

        Notes = notes?.Trim();
        if (Notes?.Length > MaxNotesLength)
            Notes = Notes.Substring(0, MaxNotesLength);

        QueueDomainEvent(new ChartOfAccountCreated(Id, AccountCode, AccountName, AccountType, UsoaCategory, Description, Notes));
    }

    public static ChartOfAccount Create(string accountId, string accountName, string accountType, 
        string usoaCategory, DefaultIdType? subAccountOf = null, string? parentCode = null,
        decimal balance = 0, bool isControlAccount = false, string normalBalance = "Debit",
        bool isUsoaCompliant = true, string? regulatoryClassification = null,
        string? description = null, string? notes = null)
    {
        return new ChartOfAccount(accountId, accountName, accountType, usoaCategory,
            subAccountOf, parentCode, balance, isControlAccount, normalBalance, isUsoaCompliant,
            regulatoryClassification, description, notes);
    }

    public ChartOfAccount Update(string? accountName = null, string? accountType = null, 
        string? usoaCategory = null, DefaultIdType? subAccountOf = null, string? parentCode = null,
        bool? isControlAccount = null, string? normalBalance = null, bool? isUsoaCompliant = null,
        string? regulatoryClassification = null, string? description = null, string? notes = null)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(accountName) && AccountName != accountName.Trim())
        {
            var an = accountName.Trim();
            if (an.Length > MaxAccountNameLength)
                throw new ChartOfAccountInvalidException($"Account name cannot exceed {MaxAccountNameLength} characters.");
            Name = an;
            AccountName = an;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(accountType) && AccountType != accountType.Trim())
        {
            var at = accountType.Trim();
            if (!IsValidAccountType(at))
                throw new ChartOfAccountInvalidException($"Invalid account type: {accountType}");
            if (at.Length > MaxAccountTypeLength)
                throw new ChartOfAccountInvalidException($"Account type cannot exceed {MaxAccountTypeLength} characters.");
            AccountType = at;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(usoaCategory) && UsoaCategory != usoaCategory.Trim())
        {
            var uc = usoaCategory.Trim();
            if (!IsValidUsoaCategory(uc))
                throw new ChartOfAccountInvalidException($"Invalid USOA category: {usoaCategory}");
            if (uc.Length > MaxUsoaCategoryLength)
                throw new ChartOfAccountInvalidException($"USOA category cannot exceed {MaxUsoaCategoryLength} characters.");
            UsoaCategory = uc;
            isUpdated = true;
        }

        if (subAccountOf != SubAccountOf)
        {
            SubAccountOf = subAccountOf;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(parentCode) && ParentCode != parentCode.Trim())
        {
            var pc = parentCode.Trim();
            if (pc.Length > MaxParentCodeLength)
                pc = pc.Substring(0, MaxParentCodeLength);
            ParentCode = pc;
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
            var nb = normalBalance.Trim();
            if (nb.Length > MaxNormalBalanceLength)
                nb = nb.Substring(0, MaxNormalBalanceLength);
            NormalBalance = nb;
            isUpdated = true;
        }

        if (isUsoaCompliant.HasValue && IsUsoaCompliant != isUsoaCompliant.Value)
        {
            IsUsoaCompliant = isUsoaCompliant.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(regulatoryClassification) && RegulatoryClassification != regulatoryClassification.Trim())
        {
            var rc = regulatoryClassification.Trim();
            if (rc.Length > MaxRegulatoryClassificationLength)
                rc = rc.Substring(0, MaxRegulatoryClassificationLength);
            RegulatoryClassification = rc;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description?.Trim())
        {
            var d = description.Trim();
            if (d.Length > MaxDescriptionLength)
                d = d.Substring(0, MaxDescriptionLength);
            Description = d;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(notes) && Notes != notes?.Trim())
        {
            var n = notes.Trim();
            if (n.Length > MaxNotesLength)
                n = n.Substring(0, MaxNotesLength);
            Notes = n;
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
