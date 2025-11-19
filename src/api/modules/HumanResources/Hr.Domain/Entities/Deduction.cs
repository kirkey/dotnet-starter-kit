namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a deduction type master data (loans, cash advances, uniform deductions, etc).
/// Defines deduction rules, calculation methods, and recovery schedules per Philippines Labor Code.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Configurable deduction types (Loan, Cash Advance, Uniform, Equipment, Damages)
/// - Recovery schedule rules (one-time, installment-based, percentage-based)
/// - Compliance with Philippines Labor Code Art 113 (wage deduction limitations)
/// - Maximum deduction percentage enforcement (20% net salary limit per DOLE)
/// 
/// Examples:
/// - SSS Loan: Installment-based, 24 months max, auto-deduct from payroll
/// - Cash Advance: Percentage-based recovery, 10% per payroll
/// - Uniform Deduction: One-time or installment
/// - Damages: Requires authorization, installment allowed
/// </remarks>
public class Deduction : AuditableEntity, IAggregateRoot
{
    private Deduction() { }

    private Deduction(
        DefaultIdType id,
        string deductionName,
        string deductionType,
        string recoveryMethod = "Manual")
    {
        Id = id;
        DeductionName = deductionName;
        DeductionType = deductionType;
        RecoveryMethod = recoveryMethod;
        IsActive = true;
        RequiresApproval = true;
        IsRecurring = false;
        MaxRecoveryPercentage = 20.00m; // DOLE limit: 20% of net salary
    }

    /// <summary>
    /// Name of the deduction (SSS Loan, Cash Advance, Uniform, Equipment, etc).
    /// </summary>
    public string DeductionName { get; private set; } = default!;

    /// <summary>
    /// Type of deduction: Loan, CashAdvance, Uniform, Equipment, Damages, Others.
    /// </summary>
    public string DeductionType { get; private set; } = default!;

    /// <summary>
    /// Recovery method: Manual, FixedAmount, Percentage, Installment.
    /// - Manual: HR manually applies deduction each period
    /// - FixedAmount: Fixed amount per payroll period
    /// - Percentage: Percentage of gross/net salary
    /// - Installment: Equal installments over defined periods
    /// </summary>
    public string RecoveryMethod { get; private set; } = default!;

    /// <summary>
    /// Fixed recovery amount per payroll period (if RecoveryMethod = FixedAmount).
    /// </summary>
    public decimal? RecoveryFixedAmount { get; private set; }

    /// <summary>
    /// Recovery percentage of salary (if RecoveryMethod = Percentage).
    /// Example: 10.00 = 10% of net salary per payroll.
    /// </summary>
    public decimal? RecoveryPercentage { get; private set; }

    /// <summary>
    /// Number of installments (if RecoveryMethod = Installment).
    /// Example: 12 months for SSS loan.
    /// </summary>
    public int? InstallmentCount { get; private set; }

    /// <summary>
    /// Maximum recovery percentage per payroll period (DOLE Art 113 limit: 20% net salary).
    /// Ensures compliance with wage deduction laws.
    /// </summary>
    public decimal MaxRecoveryPercentage { get; private set; }

    /// <summary>
    /// Whether manager approval is required before applying deduction.
    /// Required for: Damages, Equipment losses, Uniform (if not standard issue).
    /// Not required for: Authorized loans (SSS, Pag-IBIG), pre-approved cash advances.
    /// </summary>
    public bool RequiresApproval { get; private set; }

    /// <summary>
    /// Whether this deduction recurs automatically each payroll period.
    /// True: SSS Loan (auto-deduct), Uniform (installment plan)
    /// False: One-time deduction (damages, cash advance recovery completion)
    /// </summary>
    public bool IsRecurring { get; private set; }

    /// <summary>
    /// Whether this deduction type is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// GL account code for posting deduction to general ledger.
    /// Example: "2110" for Employee Loans Receivable.
    /// </summary>
    public string? GlAccountCode { get; private set; }

    /// <summary>
    /// Description of the deduction type and recovery rules.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Creates a new deduction type.
    /// </summary>
    public static Deduction Create(
        string deductionName,
        string deductionType,
        string recoveryMethod = "Manual")
    {
        if (string.IsNullOrWhiteSpace(deductionName))
            throw new ArgumentException("Deduction name is required.", nameof(deductionName));

        if (string.IsNullOrWhiteSpace(deductionType))
            throw new ArgumentException("Deduction type is required.", nameof(deductionType));

        return new Deduction(
            DefaultIdType.NewGuid(),
            deductionName,
            deductionType,
            recoveryMethod);
    }

    /// <summary>
    /// Updates deduction information.
    /// </summary>
    public Deduction Update(
        string? deductionName = null,
        string? deductionType = null,
        string? recoveryMethod = null,
        string? glAccountCode = null,
        string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(deductionName))
            DeductionName = deductionName;

        if (!string.IsNullOrWhiteSpace(deductionType))
            DeductionType = deductionType;

        if (!string.IsNullOrWhiteSpace(recoveryMethod))
            RecoveryMethod = recoveryMethod;

        if (glAccountCode != null)
            GlAccountCode = glAccountCode;

        if (description != null)
            Description = description;

        return this;
    }

    /// <summary>
    /// Sets recovery method details based on method type.
    /// </summary>
    public Deduction SetRecoveryDetails(
        decimal? fixedAmount = null,
        decimal? percentage = null,
        int? installmentCount = null)
    {
        if (fixedAmount.HasValue && fixedAmount.Value > 0)
            RecoveryFixedAmount = fixedAmount.Value;

        if (percentage.HasValue && percentage.Value > 0 && percentage.Value <= 100)
            RecoveryPercentage = percentage.Value;

        if (installmentCount.HasValue && installmentCount.Value > 0)
            InstallmentCount = installmentCount.Value;

        return this;
    }

    /// <summary>
    /// Sets maximum recovery percentage (DOLE compliance).
    /// </summary>
    public Deduction SetMaxRecoveryPercentage(decimal maxPercentage)
    {
        if (maxPercentage <= 0 || maxPercentage > 100)
            throw new ArgumentException("Max recovery percentage must be between 0 and 100.", nameof(maxPercentage));

        MaxRecoveryPercentage = maxPercentage;
        return this;
    }

    /// <summary>
    /// Sets whether approval is required.
    /// </summary>
    public Deduction SetRequiresApproval(bool requiresApproval = true)
    {
        RequiresApproval = requiresApproval;
        return this;
    }

    /// <summary>
    /// Sets whether deduction is recurring.
    /// </summary>
    public Deduction SetIsRecurring(bool isRecurring = true)
    {
        IsRecurring = isRecurring;
        return this;
    }

    /// <summary>
    /// Activates the deduction type.
    /// </summary>
    public Deduction Activate()
    {
        IsActive = true;
        return this;
    }

    /// <summary>
    /// Deactivates the deduction type.
    /// </summary>
    public Deduction Deactivate()
    {
        IsActive = false;
        return this;
    }
}

