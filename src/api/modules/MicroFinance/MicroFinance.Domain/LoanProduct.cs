using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan product template in the microfinance system.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Define standard loan offerings (e.g., Agricultural Loan, Emergency Loan, Business Loan)</description></item>
///   <item><description>Configure interest rates, terms, and repayment structures for loan types</description></item>
///   <item><description>Set borrowing limits (min/max amounts) and eligibility criteria</description></item>
///   <item><description>Standardize late payment penalties and grace periods</description></item>
///   <item><description>Enable/disable loan products without affecting existing loans</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Loan products serve as templates from which individual loans are created. When a member applies for a loan,
/// they select a product, and the loan inherits the product's terms. This allows MFIs to maintain consistent
/// lending policies while offering multiple loan types tailored to different needs (agriculture, education, 
/// micro-enterprise, emergency, etc.).
/// </para>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Loan"/> - Individual loans created from this product template</description></item>
///   <item><description><see cref="Member"/> - Members who borrow using this product</description></item>
///   <item><description><see cref="FeeDefinition"/> - Fees that may apply to this product type</description></item>
/// </list>
/// </remarks>
public class LoanProduct : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for product code. (2^6 = 64)</summary>
    public const int CodeMaxLength = 64;

    /// <summary>Maximum length for product name. (2^8 = 256)</summary>
    public const int NameMaxLength = 256;

    /// <summary>Maximum length for description. (2^11 = 2048)</summary>
    public const int DescriptionMaxLength = 2048;

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

    /// <summary>
    /// Gets the unique product code.
    /// </summary>
    /// <remarks>
    /// A short, unique identifier for internal reference and reporting (e.g., "AGRI-01", "EMERG-001").
    /// Used for quick lookup and integration with external systems.
    /// </remarks>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Gets the product name.
    /// </summary>
    /// <remarks>
    /// A human-readable name displayed to users (e.g., "Agricultural Season Loan", "Emergency Fund").
    /// </remarks>
    public new string Name { get; private set; } = default!;

    /// <summary>
    /// Gets the product description.
    /// </summary>
    /// <remarks>
    /// Detailed description of the loan product including purpose, target beneficiaries, and special conditions.
    /// </remarks>
    public new string? Description { get; private set; }

    /// <summary>
    /// Gets the minimum loan amount a member can borrow.
    /// </summary>
    /// <remarks>
    /// Prevents loans that are too small to be operationally cost-effective.
    /// </remarks>
    public decimal MinLoanAmount { get; private set; }

    /// <summary>
    /// Gets the maximum loan amount a member can borrow.
    /// </summary>
    /// <remarks>
    /// Risk management control to limit exposure per borrower. May be further limited by member creditworthiness.
    /// </remarks>
    public decimal MaxLoanAmount { get; private set; }

    /// <summary>
    /// Gets the annual interest rate (percentage).
    /// </summary>
    /// <remarks>
    /// The base interest rate applied to loans. Actual interest charged depends on <see cref="InterestMethod"/>.
    /// Expressed as a percentage (e.g., 12.5 for 12.5% per annum).
    /// </remarks>
    public decimal InterestRate { get; private set; }

    /// <summary>
    /// Gets the interest calculation method.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///   <item><description><strong>Flat</strong>: Interest calculated on original principal throughout the term</description></item>
    ///   <item><description><strong>Declining</strong>: Interest calculated on outstanding balance (reduces over time)</description></item>
    ///   <item><description><strong>Compound</strong>: Interest calculated on principal plus accumulated interest</description></item>
    /// </list>
    /// Declining balance is more borrower-friendly; Flat is simpler to administer.
    /// </remarks>
    public string InterestMethod { get; private set; } = default!;

    /// <summary>
    /// Gets the minimum loan term in months.
    /// </summary>
    /// <remarks>
    /// Shortest repayment period allowed. Shorter terms mean higher periodic payments but less total interest.
    /// </remarks>
    public int MinTermMonths { get; private set; }

    /// <summary>
    /// Gets the maximum loan term in months.
    /// </summary>
    /// <remarks>
    /// Longest repayment period allowed. Longer terms mean lower periodic payments but more total interest.
    /// </remarks>
    public int MaxTermMonths { get; private set; }

    /// <summary>
    /// Gets the repayment frequency.
    /// </summary>
    /// <remarks>
    /// How often the borrower must make payments:
    /// <list type="bullet">
    ///   <item><description><strong>Daily</strong>: Common for micro-vendors with daily cash flow</description></item>
    ///   <item><description><strong>Weekly</strong>: Popular for group lending (solidarity groups)</description></item>
    ///   <item><description><strong>Biweekly</strong>: Aligned with typical pay cycles</description></item>
    ///   <item><description><strong>Monthly</strong>: Standard for salaried borrowers</description></item>
    /// </list>
    /// </remarks>
    public string RepaymentFrequency { get; private set; } = default!;

    /// <summary>
    /// Gets the grace period in days before first repayment.
    /// </summary>
    /// <remarks>
    /// Time after disbursement before the first payment is due. Useful for agricultural loans
    /// where income comes after harvest, or business loans needing time to generate revenue.
    /// </remarks>
    public int GracePeriodDays { get; private set; }

    /// <summary>
    /// Gets the late payment penalty rate (percentage).
    /// </summary>
    /// <remarks>
    /// Additional interest charged on overdue amounts. Applied as a percentage of the overdue installment.
    /// Encourages timely repayment and compensates for increased administrative costs.
    /// </remarks>
    public decimal LatePenaltyRate { get; private set; }

    /// <summary>
    /// Gets whether the product is currently active and available for new loans.
    /// </summary>
    /// <remarks>
    /// When deactivated, no new loans can be created from this product, but existing loans continue normally.
    /// Useful for phasing out products or seasonal offerings.
    /// </remarks>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the collection of loans created from this product template.
    /// </summary>
    /// <remarks>
    /// Navigation property for accessing all loans using this product. Used for reporting and analytics.
    /// </remarks>
    public virtual ICollection<Loan> Loans { get; private set; } = new List<Loan>();

    private LoanProduct() { }

    private LoanProduct(
        DefaultIdType id,
        string code,
        string name,
        string? description,
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

