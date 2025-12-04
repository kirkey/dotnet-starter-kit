using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan product template in the microfinance system.
/// Defines the terms and conditions for a type of loan.
/// </summary>
public class LoanProduct : AuditableEntity, IAggregateRoot
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

    /// <summary>Maximum length for repayment frequency. (2^5 = 32)</summary>
    public const int RepaymentFrequencyMaxLength = 32;

    /// <summary>Maximum length for interest method. (2^5 = 32)</summary>
    public const int InterestMethodMaxLength = 32;

    /// <summary>Minimum length for product name.</summary>
    public const int NameMinLength = 2;

    /// <summary>Minimum interest rate (0%).</summary>
    public const decimal MinInterestRate = 0m;

    /// <summary>Maximum interest rate (100%).</summary>
    public const decimal MaxInterestRate = 100m;

    /// <summary>Gets the unique product code.</summary>
    public string Code { get; private set; } = default!;

    /// <summary>Gets the product name.</summary>
    public new string Name { get; private set; } = default!;

    /// <summary>Gets the product description.</summary>
    public new string? Description { get; private set; }

    /// <summary>Gets the currency code (e.g., USD, EUR).</summary>
    public string CurrencyCode { get; private set; } = default!;

    /// <summary>Gets the minimum loan amount.</summary>
    public decimal MinLoanAmount { get; private set; }

    /// <summary>Gets the maximum loan amount.</summary>
    public decimal MaxLoanAmount { get; private set; }

    /// <summary>Gets the annual interest rate (percentage).</summary>
    public decimal InterestRate { get; private set; }

    /// <summary>Gets the interest calculation method (Flat, Declining, etc.).</summary>
    public string InterestMethod { get; private set; } = default!;

    /// <summary>Gets the minimum loan term in months.</summary>
    public int MinTermMonths { get; private set; }

    /// <summary>Gets the maximum loan term in months.</summary>
    public int MaxTermMonths { get; private set; }

    /// <summary>Gets the repayment frequency (Daily, Weekly, Monthly, etc.).</summary>
    public string RepaymentFrequency { get; private set; } = default!;

    /// <summary>Gets the grace period in days before first repayment.</summary>
    public int GracePeriodDays { get; private set; }

    /// <summary>Gets the late payment penalty rate (percentage).</summary>
    public decimal LatePenaltyRate { get; private set; }

    /// <summary>Gets whether the product is active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the collection of loans using this product.</summary>
    public virtual ICollection<Loan> Loans { get; private set; } = new List<Loan>();

    private LoanProduct() { }

    private LoanProduct(
        DefaultIdType id,
        string code,
        string name,
        string? description,
        string currencyCode,
        decimal minLoanAmount,
        decimal maxLoanAmount,
        decimal interestRate,
        string interestMethod,
        int minTermMonths,
        int maxTermMonths,
        string repaymentFrequency,
        int gracePeriodDays,
        decimal latePenaltyRate)
    {
        Id = id;
        Code = code.Trim();
        ValidateAndSetName(name);
        Description = description?.Trim();
        CurrencyCode = currencyCode.Trim().ToUpperInvariant();
        MinLoanAmount = minLoanAmount;
        MaxLoanAmount = maxLoanAmount;
        ValidateAndSetInterestRate(interestRate);
        InterestMethod = interestMethod.Trim();
        MinTermMonths = minTermMonths;
        MaxTermMonths = maxTermMonths;
        RepaymentFrequency = repaymentFrequency.Trim();
        GracePeriodDays = gracePeriodDays;
        LatePenaltyRate = latePenaltyRate;
        IsActive = true;

        QueueDomainEvent(new LoanProductCreated { LoanProduct = this });
    }

    /// <summary>
    /// Creates a new LoanProduct instance.
    /// </summary>
    public static LoanProduct Create(
        string code,
        string name,
        string? description,
        string currencyCode,
        decimal minLoanAmount,
        decimal maxLoanAmount,
        decimal interestRate,
        string interestMethod,
        int minTermMonths,
        int maxTermMonths,
        string repaymentFrequency,
        int gracePeriodDays = 0,
        decimal latePenaltyRate = 0)
    {
        return new LoanProduct(
            DefaultIdType.NewGuid(),
            code,
            name,
            description,
            currencyCode,
            minLoanAmount,
            maxLoanAmount,
            interestRate,
            interestMethod,
            minTermMonths,
            maxTermMonths,
            repaymentFrequency,
            gracePeriodDays,
            latePenaltyRate);
    }

    /// <summary>
    /// Updates the loan product information.
    /// </summary>
    public LoanProduct Update(
        string? name,
        string? description,
        decimal? minLoanAmount,
        decimal? maxLoanAmount,
        decimal? interestRate,
        string? interestMethod,
        int? minTermMonths,
        int? maxTermMonths,
        string? repaymentFrequency,
        int? gracePeriodDays,
        decimal? latePenaltyRate)
    {
        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            ValidateAndSetName(name);
            hasChanges = true;
        }

        if (description != Description) { Description = description?.Trim(); hasChanges = true; }
        if (minLoanAmount.HasValue && minLoanAmount != MinLoanAmount) { MinLoanAmount = minLoanAmount.Value; hasChanges = true; }
        if (maxLoanAmount.HasValue && maxLoanAmount != MaxLoanAmount) { MaxLoanAmount = maxLoanAmount.Value; hasChanges = true; }
        if (interestRate.HasValue && interestRate != InterestRate) { ValidateAndSetInterestRate(interestRate.Value); hasChanges = true; }
        if (!string.IsNullOrWhiteSpace(interestMethod) && interestMethod != InterestMethod) { InterestMethod = interestMethod.Trim(); hasChanges = true; }
        if (minTermMonths.HasValue && minTermMonths != MinTermMonths) { MinTermMonths = minTermMonths.Value; hasChanges = true; }
        if (maxTermMonths.HasValue && maxTermMonths != MaxTermMonths) { MaxTermMonths = maxTermMonths.Value; hasChanges = true; }
        if (!string.IsNullOrWhiteSpace(repaymentFrequency) && repaymentFrequency != RepaymentFrequency) { RepaymentFrequency = repaymentFrequency.Trim(); hasChanges = true; }
        if (gracePeriodDays.HasValue && gracePeriodDays != GracePeriodDays) { GracePeriodDays = gracePeriodDays.Value; hasChanges = true; }
        if (latePenaltyRate.HasValue && latePenaltyRate != LatePenaltyRate) { LatePenaltyRate = latePenaltyRate.Value; hasChanges = true; }

        if (hasChanges)
        {
            QueueDomainEvent(new LoanProductUpdated { LoanProduct = this });
        }

        return this;
    }

    /// <summary>Activates the loan product.</summary>
    public LoanProduct Activate()
    {
        if (!IsActive) { IsActive = true; }
        return this;
    }

    /// <summary>Deactivates the loan product.</summary>
    public LoanProduct Deactivate()
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

    private void ValidateAndSetInterestRate(decimal interestRate)
    {
        if (interestRate < MinInterestRate || interestRate > MaxInterestRate)
            throw new ArgumentException($"Interest rate must be between {MinInterestRate}% and {MaxInterestRate}%.", nameof(interestRate));

        InterestRate = interestRate;
    }
}

