using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a fee or charge definition in the microfinance system.
/// Fees can be applied to loans, savings, shares, or other products.
/// </summary>
public class FeeDefinition : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for fee code. (2^6 = 64)</summary>
    public const int CodeMaxLength = 64;

    /// <summary>Maximum length for fee name. (2^8 = 256)</summary>
    public const int NameMaxLength = 256;

    /// <summary>Maximum length for description. (2^11 = 2048)</summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>Maximum length for fee type. (2^5 = 32)</summary>
    public const int FeeTypeMaxLength = 32;

    /// <summary>Maximum length for calculation type. (2^5 = 32)</summary>
    public const int CalculationTypeMaxLength = 32;

    /// <summary>Maximum length for applies to. (2^5 = 32)</summary>
    public const int AppliesToMaxLength = 32;

    /// <summary>Maximum length for charge frequency. (2^5 = 32)</summary>
    public const int ChargeFrequencyMaxLength = 32;

    /// <summary>Maximum length for currency code. (2^3 = 8)</summary>
    public const int CurrencyCodeMaxLength = 8;

    /// <summary>Minimum length for fee name.</summary>
    public const int NameMinLength = 2;

    // Fee Types
    public const string TypeDisbursement = "Disbursement";
    public const string TypeProcessing = "Processing";
    public const string TypeLateFee = "LateFee";
    public const string TypeEarlyPayment = "EarlyPayment";
    public const string TypeAccountMaintenance = "AccountMaintenance";
    public const string TypeWithdrawal = "Withdrawal";
    public const string TypeTransfer = "Transfer";
    public const string TypeClosingFee = "ClosingFee";
    public const string TypeInsurance = "Insurance";

    // Calculation Types
    public const string CalculationFlat = "Flat";
    public const string CalculationPercentage = "Percentage";
    public const string CalculationPerTransaction = "PerTransaction";

    // Applies To
    public const string AppliesLoan = "Loan";
    public const string AppliesSavings = "Savings";
    public const string AppliesShares = "Shares";
    public const string AppliesFixedDeposit = "FixedDeposit";
    public const string AppliesMember = "Member";

    // Charge Frequency
    public const string FrequencyOneTime = "OneTime";
    public const string FrequencyMonthly = "Monthly";
    public const string FrequencyQuarterly = "Quarterly";
    public const string FrequencyAnnually = "Annually";
    public const string FrequencyPerEvent = "PerEvent";

    /// <summary>Gets the unique fee code.</summary>
    public string Code { get; private set; } = default!;

    /// <summary>Gets the fee name.</summary>
    public new string Name { get; private set; } = default!;

    /// <summary>Gets the fee description.</summary>
    public new string? Description { get; private set; }

    /// <summary>Gets the fee type.</summary>
    public string FeeType { get; private set; } = default!;

    /// <summary>Gets the calculation type (Flat, Percentage).</summary>
    public string CalculationType { get; private set; } = default!;

    /// <summary>Gets what this fee applies to (Loan, Savings, etc.).</summary>
    public string AppliesTo { get; private set; } = default!;

    /// <summary>Gets the charge frequency.</summary>
    public string ChargeFrequency { get; private set; } = default!;

    /// <summary>Gets the fee amount (flat) or rate (percentage).</summary>
    public decimal Amount { get; private set; }

    /// <summary>Gets the minimum fee amount (for percentage-based fees).</summary>
    public decimal? MinAmount { get; private set; }

    /// <summary>Gets the maximum fee amount (for percentage-based fees).</summary>
    public decimal? MaxAmount { get; private set; }

    /// <summary>Gets the currency code.</summary>
    public string CurrencyCode { get; private set; } = default!;

    /// <summary>Gets whether the fee is taxable.</summary>
    public bool IsTaxable { get; private set; }

    /// <summary>Gets the tax rate if taxable.</summary>
    public decimal? TaxRate { get; private set; }

    /// <summary>Gets whether the product is active.</summary>
    public bool IsActive { get; private set; }

    private FeeDefinition() { }

    private FeeDefinition(
        DefaultIdType id,
        string code,
        string name,
        string? description,
        string feeType,
        string calculationType,
        string appliesTo,
        string chargeFrequency,
        decimal amount,
        decimal? minAmount,
        decimal? maxAmount,
        string currencyCode,
        bool isTaxable,
        decimal? taxRate)
    {
        Id = id;
        Code = code.Trim();
        ValidateAndSetName(name);
        Description = description?.Trim();
        FeeType = feeType.Trim();
        CalculationType = calculationType.Trim();
        AppliesTo = appliesTo.Trim();
        ChargeFrequency = chargeFrequency.Trim();
        Amount = amount;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        CurrencyCode = currencyCode.Trim().ToUpperInvariant();
        IsTaxable = isTaxable;
        TaxRate = taxRate;
        IsActive = true;

        QueueDomainEvent(new FeeDefinitionCreated { FeeDefinition = this });
    }

    /// <summary>
    /// Creates a new FeeDefinition instance.
    /// </summary>
    public static FeeDefinition Create(
        string code,
        string name,
        string feeType,
        string calculationType,
        string appliesTo,
        string chargeFrequency,
        decimal amount,
        string currencyCode,
        string? description = null,
        decimal? minAmount = null,
        decimal? maxAmount = null,
        bool isTaxable = false,
        decimal? taxRate = null)
    {
        return new FeeDefinition(
            DefaultIdType.NewGuid(),
            code,
            name,
            description,
            feeType,
            calculationType,
            appliesTo,
            chargeFrequency,
            amount,
            minAmount,
            maxAmount,
            currencyCode,
            isTaxable,
            taxRate);
    }

    /// <summary>
    /// Updates the fee definition.
    /// </summary>
    public FeeDefinition Update(
        string? name,
        string? description,
        decimal? amount,
        decimal? minAmount,
        decimal? maxAmount,
        bool? isTaxable,
        decimal? taxRate)
    {
        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            ValidateAndSetName(name);
            hasChanges = true;
        }

        if (description != Description) { Description = description?.Trim(); hasChanges = true; }
        if (amount.HasValue && amount != Amount) { Amount = amount.Value; hasChanges = true; }
        if (minAmount != MinAmount) { MinAmount = minAmount; hasChanges = true; }
        if (maxAmount != MaxAmount) { MaxAmount = maxAmount; hasChanges = true; }
        if (isTaxable.HasValue && isTaxable != IsTaxable) { IsTaxable = isTaxable.Value; hasChanges = true; }
        if (taxRate != TaxRate) { TaxRate = taxRate; hasChanges = true; }

        if (hasChanges)
        {
            QueueDomainEvent(new FeeDefinitionUpdated { FeeDefinition = this });
        }

        return this;
    }

    /// <summary>
    /// Calculates the fee amount based on a principal/base amount.
    /// </summary>
    public decimal CalculateFee(decimal baseAmount)
    {
        decimal fee;
        
        if (CalculationType.Equals(CalculationPercentage, StringComparison.OrdinalIgnoreCase))
        {
            fee = baseAmount * (Amount / 100m);
            
            if (MinAmount.HasValue && fee < MinAmount.Value)
                fee = MinAmount.Value;
            
            if (MaxAmount.HasValue && fee > MaxAmount.Value)
                fee = MaxAmount.Value;
        }
        else
        {
            fee = Amount;
        }

        if (IsTaxable && TaxRate.HasValue)
        {
            fee += fee * (TaxRate.Value / 100m);
        }

        return fee;
    }

    /// <summary>Activates the fee definition.</summary>
    public FeeDefinition Activate()
    {
        if (!IsActive) { IsActive = true; }
        return this;
    }

    /// <summary>Deactivates the fee definition.</summary>
    public FeeDefinition Deactivate()
    {
        if (IsActive) { IsActive = false; }
        return this;
    }

    private void ValidateAndSetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Fee name cannot be empty.", nameof(name));

        string trimmed = name.Trim();
        if (trimmed.Length < NameMinLength)
            throw new ArgumentException($"Fee name must be at least {NameMinLength} characters.", nameof(name));
        if (trimmed.Length > NameMaxLength)
            throw new ArgumentException($"Fee name cannot exceed {NameMaxLength} characters.", nameof(name));

        Name = trimmed;
    }
}
