using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a savings product template in the microfinance system.
/// Defines the terms and conditions for a type of savings account.
/// </summary>
public class SavingsProduct : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for product code. (2^6 = 64)</summary>
    public const int CodeMaxLength = 64;

    /// <summary>Maximum length for product name. (2^8 = 256)</summary>
    public const int NameMaxLength = 256;

    /// <summary>Maximum length for description. (2^11 = 2048)</summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>Maximum length for currency code. (2^3 = 8)</summary>
    public const int CurrencyCodeMaxLength = 8;

    /// <summary>Maximum length for interest calculation method. (2^5 = 32)</summary>
    public const int InterestCalculationMaxLength = 32;

    /// <summary>Maximum length for interest posting frequency. (2^5 = 32)</summary>
    public const int InterestPostingFrequencyMaxLength = 32;

    /// <summary>Minimum length for product name.</summary>
    public const int NameMinLength = 2;

    /// <summary>Gets the unique product code.</summary>
    public string Code { get; private set; } = default!;

    /// <summary>Gets the product name.</summary>
    public new string Name { get; private set; } = default!;

    /// <summary>Gets the product description.</summary>
    public new string? Description { get; private set; }

    /// <summary>Gets the currency code (e.g., USD, EUR).</summary>
    public string CurrencyCode { get; private set; } = default!;

    /// <summary>Gets the annual interest rate (percentage).</summary>
    public decimal InterestRate { get; private set; }

    /// <summary>Gets the interest calculation method (Daily, Monthly).</summary>
    public string InterestCalculation { get; private set; } = default!;

    /// <summary>Gets the interest posting frequency (Monthly, Quarterly, Annually).</summary>
    public string InterestPostingFrequency { get; private set; } = default!;

    /// <summary>Gets the minimum opening balance.</summary>
    public decimal MinOpeningBalance { get; private set; }

    /// <summary>Gets the minimum balance to earn interest.</summary>
    public decimal MinBalanceForInterest { get; private set; }

    /// <summary>Gets the minimum withdrawal amount.</summary>
    public decimal MinWithdrawalAmount { get; private set; }

    /// <summary>Gets the maximum withdrawal amount per day.</summary>
    public decimal? MaxWithdrawalPerDay { get; private set; }

    /// <summary>Gets whether the product allows overdraft.</summary>
    public bool AllowOverdraft { get; private set; }

    /// <summary>Gets the overdraft limit if allowed.</summary>
    public decimal? OverdraftLimit { get; private set; }

    /// <summary>Gets whether the product is active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the collection of savings accounts using this product.</summary>
    public virtual ICollection<SavingsAccount> SavingsAccounts { get; private set; } = new List<SavingsAccount>();

    private SavingsProduct() { }

    private SavingsProduct(
        DefaultIdType id,
        string code,
        string name,
        string? description,
        string currencyCode,
        decimal interestRate,
        string interestCalculation,
        string interestPostingFrequency,
        decimal minOpeningBalance,
        decimal minBalanceForInterest,
        decimal minWithdrawalAmount,
        decimal? maxWithdrawalPerDay,
        bool allowOverdraft,
        decimal? overdraftLimit)
    {
        Id = id;
        Code = code.Trim();
        ValidateAndSetName(name);
        Description = description?.Trim();
        CurrencyCode = currencyCode.Trim().ToUpperInvariant();
        InterestRate = interestRate;
        InterestCalculation = interestCalculation.Trim();
        InterestPostingFrequency = interestPostingFrequency.Trim();
        MinOpeningBalance = minOpeningBalance;
        MinBalanceForInterest = minBalanceForInterest;
        MinWithdrawalAmount = minWithdrawalAmount;
        MaxWithdrawalPerDay = maxWithdrawalPerDay;
        AllowOverdraft = allowOverdraft;
        OverdraftLimit = overdraftLimit;
        IsActive = true;

        QueueDomainEvent(new SavingsProductCreated { SavingsProduct = this });
    }

    /// <summary>
    /// Creates a new SavingsProduct instance.
    /// </summary>
    public static SavingsProduct Create(
        string code,
        string name,
        string? description,
        string currencyCode,
        decimal interestRate,
        string interestCalculation,
        string interestPostingFrequency,
        decimal minOpeningBalance = 0,
        decimal minBalanceForInterest = 0,
        decimal minWithdrawalAmount = 0,
        decimal? maxWithdrawalPerDay = null,
        bool allowOverdraft = false,
        decimal? overdraftLimit = null)
    {
        return new SavingsProduct(
            DefaultIdType.NewGuid(),
            code,
            name,
            description,
            currencyCode,
            interestRate,
            interestCalculation,
            interestPostingFrequency,
            minOpeningBalance,
            minBalanceForInterest,
            minWithdrawalAmount,
            maxWithdrawalPerDay,
            allowOverdraft,
            overdraftLimit);
    }

    /// <summary>
    /// Updates the savings product information.
    /// </summary>
    public SavingsProduct Update(
        string? name,
        string? description,
        decimal? interestRate,
        string? interestCalculation,
        string? interestPostingFrequency,
        decimal? minOpeningBalance,
        decimal? minBalanceForInterest,
        decimal? minWithdrawalAmount,
        decimal? maxWithdrawalPerDay,
        bool? allowOverdraft,
        decimal? overdraftLimit)
    {
        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            ValidateAndSetName(name);
            hasChanges = true;
        }

        if (description != Description) { Description = description?.Trim(); hasChanges = true; }
        if (interestRate.HasValue && interestRate != InterestRate) { InterestRate = interestRate.Value; hasChanges = true; }
        if (!string.IsNullOrWhiteSpace(interestCalculation) && interestCalculation != InterestCalculation) { InterestCalculation = interestCalculation.Trim(); hasChanges = true; }
        if (!string.IsNullOrWhiteSpace(interestPostingFrequency) && interestPostingFrequency != InterestPostingFrequency) { InterestPostingFrequency = interestPostingFrequency.Trim(); hasChanges = true; }
        if (minOpeningBalance.HasValue && minOpeningBalance != MinOpeningBalance) { MinOpeningBalance = minOpeningBalance.Value; hasChanges = true; }
        if (minBalanceForInterest.HasValue && minBalanceForInterest != MinBalanceForInterest) { MinBalanceForInterest = minBalanceForInterest.Value; hasChanges = true; }
        if (minWithdrawalAmount.HasValue && minWithdrawalAmount != MinWithdrawalAmount) { MinWithdrawalAmount = minWithdrawalAmount.Value; hasChanges = true; }
        if (maxWithdrawalPerDay != MaxWithdrawalPerDay) { MaxWithdrawalPerDay = maxWithdrawalPerDay; hasChanges = true; }
        if (allowOverdraft.HasValue && allowOverdraft != AllowOverdraft) { AllowOverdraft = allowOverdraft.Value; hasChanges = true; }
        if (overdraftLimit != OverdraftLimit) { OverdraftLimit = overdraftLimit; hasChanges = true; }

        if (hasChanges)
        {
            QueueDomainEvent(new SavingsProductUpdated { SavingsProduct = this });
        }

        return this;
    }

    /// <summary>Activates the savings product.</summary>
    public SavingsProduct Activate()
    {
        if (!IsActive) { IsActive = true; }
        return this;
    }

    /// <summary>Deactivates the savings product.</summary>
    public SavingsProduct Deactivate()
    {
        if (IsActive) { IsActive = false; }
        return this;
    }

    private void ValidateAndSetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));

        string trimmed = name.Trim();
        if (trimmed.Length < NameMinLength)
            throw new ArgumentException($"Product name must be at least {NameMinLength} characters.", nameof(name));
        if (trimmed.Length > NameMaxLength)
            throw new ArgumentException($"Product name cannot exceed {NameMaxLength} characters.", nameof(name));

        Name = trimmed;
    }
}

