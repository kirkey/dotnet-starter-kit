using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a savings product template in the microfinance system.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Define different savings account types (e.g., Regular Savings, Minor's Account, Emergency Fund)</description></item>
///   <item><description>Configure interest rates and posting frequencies for each product type</description></item>
///   <item><description>Set minimum balance requirements and withdrawal limits</description></item>
///   <item><description>Enable overdraft facilities for qualifying products</description></item>
///   <item><description>Manage product lifecycle (activate/deactivate) without affecting existing accounts</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Savings products define the terms for member savings accounts. MFIs typically offer multiple savings products
/// to serve different needs: compulsory savings (required for loan eligibility), voluntary savings (flexible deposits),
/// target savings (goal-based), and children's accounts. Interest rates incentivize saving behavior while
/// withdrawal rules balance liquidity needs with operational costs.
/// </para>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="SavingsAccount"/> - Individual accounts created from this product template</description></item>
///   <item><description><see cref="SavingsTransaction"/> - Transactions on accounts using this product</description></item>
///   <item><description><see cref="FixedDeposit"/> - May link to savings products for interest rate reference</description></item>
///   <item><description><see cref="FeeDefinition"/> - Account maintenance and transaction fees</description></item>
/// </list>
/// </remarks>
public class SavingsProduct : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for product code. (2^6 = 64)</summary>
    public const int CodeMaxLength = 64;

    /// <summary>Maximum length for product name. (2^8 = 256)</summary>
    public const int NameMaxLength = 256;

    /// <summary>Maximum length for description. (2^11 = 2048)</summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>Maximum length for interest calculation method. (2^5 = 32)</summary>
    public const int InterestCalculationMaxLength = 32;

    /// <summary>Maximum length for interest posting frequency. (2^5 = 32)</summary>
    public const int InterestPostingFrequencyMaxLength = 32;

    /// <summary>Minimum length for product name.</summary>
    public const int NameMinLength = 2;

    /// <summary>
    /// Gets the unique product code.
    /// </summary>
    /// <remarks>
    /// Short identifier for internal reference (e.g., "SAV-REG", "SAV-COMP", "SAV-CHILD").
    /// </remarks>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Gets the annual interest rate paid on deposits (percentage).
    /// </summary>
    /// <remarks>
    /// Interest earned by the depositor. Higher rates attract savings but increase MFI costs.
    /// </remarks>
    public decimal InterestRate { get; private set; }

    /// <summary>
    /// Gets the interest calculation method.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///   <item><description><strong>Daily</strong>: Interest calculated on daily closing balance</description></item>
    ///   <item><description><strong>Monthly</strong>: Interest calculated on average monthly balance</description></item>
    ///   <item><description><strong>MinimumBalance</strong>: Interest on lowest balance during the period</description></item>
    /// </list>
    /// </remarks>
    public string InterestCalculation { get; private set; } = default!;

    /// <summary>
    /// Gets the interest posting frequency.
    /// </summary>
    /// <remarks>
    /// How often accrued interest is credited to the account:
    /// <list type="bullet">
    ///   <item><description><strong>Monthly</strong>: Best for active accounts</description></item>
    ///   <item><description><strong>Quarterly</strong>: Common for regular savings</description></item>
    ///   <item><description><strong>Annually</strong>: Used for long-term savings</description></item>
    /// </list>
    /// </remarks>
    public string InterestPostingFrequency { get; private set; } = default!;

    /// <summary>
    /// Gets the minimum opening balance required.
    /// </summary>
    /// <remarks>
    /// Initial deposit required to open an account. May be waived for certain member categories.
    /// </remarks>
    public decimal MinOpeningBalance { get; private set; }

    /// <summary>
    /// Gets the minimum balance to earn interest.
    /// </summary>
    /// <remarks>
    /// Accounts below this threshold do not accrue interest. Encourages maintaining adequate balances.
    /// </remarks>
    public decimal MinBalanceForInterest { get; private set; }

    /// <summary>
    /// Gets the minimum withdrawal amount.
    /// </summary>
    /// <remarks>
    /// Smallest amount that can be withdrawn per transaction. Reduces processing costs for tiny transactions.
    /// </remarks>
    public decimal MinWithdrawalAmount { get; private set; }

    /// <summary>
    /// Gets the maximum withdrawal amount per day.
    /// </summary>
    /// <remarks>
    /// Daily withdrawal limit for fraud prevention and liquidity management. Null means no limit.
    /// </remarks>
    public decimal? MaxWithdrawalPerDay { get; private set; }

    /// <summary>
    /// Gets whether the product allows overdraft (negative balance).
    /// </summary>
    /// <remarks>
    /// If true, members can withdraw more than their balance up to <see cref="OverdraftLimit"/>.
    /// Typically requires good standing and incurs higher fees.
    /// </remarks>
    public bool AllowOverdraft { get; private set; }

    /// <summary>
    /// Gets the overdraft limit if allowed.
    /// </summary>
    /// <remarks>
    /// Maximum negative balance permitted. Interest is typically charged on overdraft amounts.
    /// </remarks>
    public decimal? OverdraftLimit { get; private set; }

    /// <summary>
    /// Gets whether the product is available for new accounts.
    /// </summary>
    /// <remarks>
    /// Deactivated products cannot be used for new accounts but existing accounts continue normally.
    /// </remarks>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the collection of savings accounts using this product.
    /// </summary>
    public virtual ICollection<SavingsAccount> SavingsAccounts { get; private set; } = new List<SavingsAccount>();

    private SavingsProduct() { }

    private SavingsProduct(
        DefaultIdType id,
        string code,
        string name,
        string? description,
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

