using Accounting.Domain.Events.InterconnectionAgreement;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a distributed energy resource (DER) interconnection agreement for net metering, solar, wind, and other customer-owned generation systems.
/// </summary>
/// <remarks>
/// Use cases:
/// - Manage interconnection agreements for customer-owned solar, wind, and battery systems.
/// - Track net metering credits and excess generation billing.
/// - Support multiple generation types and capacity limits.
/// - Enable monthly service charges and interconnection fees.
/// - Track generation history and annual production limits.
/// - Manage system equipment details for maintenance and inspections.
/// - Support deposit requirements for system decommissioning.
/// - Enable regulatory compliance reporting (PURPA, state mandates).
/// 
/// Default values:
/// - AgreementNumber: required unique identifier (example: "IA-2025-001234")
/// - GenerationType: required (Solar, Wind, Battery, Hydro, etc.)
/// - AgreementStatus: "Active" (new agreements start as active)
/// - InstalledCapacityKW: required system capacity in kilowatts
/// - NetMeteringRate: required rate for net metering credits
/// - ExcessGenerationRate: rate for excess generation beyond net metering
/// - MonthlyServiceCharge: 0.00 (optional monthly fee)
/// - CurrentCreditBalance: 0.00 (no initial credits)
/// - YearToDateGeneration: 0.00 (no generation initially)
/// - LifetimeGeneration: 0.00 (cumulative over agreement life)
/// - IsActive: true (agreements are active by default)
/// 
/// Business rules:
/// - AgreementNumber must be unique within the system
/// - InstalledCapacityKW must be positive and within utility limits
/// - NetMeteringRate typically equals retail rate
/// - ExcessGenerationRate often lower than net metering rate
/// - Annual generation limit enforced for net metering eligibility
/// - Monthly service charges apply even with zero consumption
/// - Credit balance carries forward month-to-month
/// - Equipment details required for interconnection approval
/// - Termination requires proper decommissioning
/// - Status transitions: Draft → Approved → Active → Suspended → Terminated
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.InterconnectionAgreement.InterconnectionAgreementCreated"/>
/// <seealso cref="Accounting.Domain.Events.InterconnectionAgreement.InterconnectionAgreementActivated"/>
/// <seealso cref="Accounting.Domain.Events.InterconnectionAgreement.InterconnectionAgreementSuspended"/>
/// <seealso cref="Accounting.Domain.Events.InterconnectionAgreement.GenerationRecorded"/>
/// <seealso cref="Accounting.Domain.Events.InterconnectionAgreement.CreditApplied"/>
public class InterconnectionAgreement : AuditableEntity, IAggregateRoot
{
    private const int MaxAgreementNumberLength = 50;
    private const int MaxGenerationTypeLength = 50;
    private const int MaxAgreementStatusLength = 32;
    private const int MaxInverterManufacturerLength = 100;
    private const int MaxInverterModelLength = 100;
    private const int MaxPanelManufacturerLength = 100;
    private const int MaxPanelModelLength = 100;
    private const int MaxTerminationReasonLength = 500;
    private const int MaxDescriptionLength = 2048;
    private const int MaxNotesLength = 2048;

    /// <summary>
    /// Unique interconnection agreement number.
    /// Example: "IA-2025-001234", "SOLAR-10001". Max length: 50.
    /// </summary>
    public string AgreementNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Reference to the member who owns this generation system.
    /// Links to Member entity. Required for all agreements.
    /// </summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>
    /// Type of generation system.
    /// Values: "Solar", "Wind", "Battery", "Hydro", "Biogas", "Combined".
    /// Required. Max length: 50.
    /// </summary>
    public string GenerationType { get; private set; } = string.Empty;

    /// <summary>
    /// Current status of the interconnection agreement.
    /// Values: "Draft", "Approved", "Active", "Suspended", "Terminated".
    /// Default: "Active". Max length: 32.
    /// </summary>
    public string AgreementStatus { get; private set; } = string.Empty;

    /// <summary>
    /// Date the agreement becomes effective.
    /// Example: 2025-01-15. Typically the commissioning date.
    /// </summary>
    public DateTime EffectiveDate { get; private set; }

    /// <summary>
    /// Date the agreement expires or was terminated.
    /// Example: 2050-01-14 for 25-year agreement. Null if no expiration.
    /// </summary>
    public DateTime? ExpirationDate { get; private set; }

    /// <summary>
    /// Installed capacity of the generation system in kilowatts.
    /// Example: 10.5 for 10.5 kW solar system. Must be positive.
    /// Used for capacity fees and production limits.
    /// </summary>
    public decimal InstalledCapacityKW { get; private set; }

    /// <summary>
    /// Rate per kWh for net metering credits.
    /// Example: 0.12 for $0.12/kWh. Typically equals retail rate.
    /// Used to calculate credit for generation offsetting consumption.
    /// </summary>
    public decimal NetMeteringRate { get; private set; }

    /// <summary>
    /// Rate per kWh for excess generation beyond net metering.
    /// Example: 0.04 for $0.04/kWh wholesale rate. Often lower than net metering.
    /// Used when generation exceeds consumption in billing period.
    /// </summary>
    public decimal ExcessGenerationRate { get; private set; }

    /// <summary>
    /// Monthly service charge for interconnection.
    /// Example: 15.00 for $15/month facility charge. Default: 0.00.
    /// Applied regardless of generation or consumption.
    /// </summary>
    public decimal MonthlyServiceCharge { get; private set; }

    /// <summary>
    /// Current net metering credit balance in dollars.
    /// Example: 150.50 for $150.50 credit. Default: 0.00.
    /// Carries forward month-to-month, may expire annually per policy.
    /// </summary>
    public decimal CurrentCreditBalance { get; private set; }

    /// <summary>
    /// Annual generation limit for net metering eligibility in kWh.
    /// Example: 12000.0 for 12,000 kWh annual limit. Null if no limit.
    /// Enforced per regulatory requirements.
    /// </summary>
    public decimal? AnnualGenerationLimit { get; private set; }

    /// <summary>
    /// Total generation recorded year-to-date in kWh.
    /// Example: 8500.75 for 8,500.75 kWh generated this year. Default: 0.00.
    /// Resets annually, used for limit enforcement.
    /// </summary>
    public decimal YearToDateGeneration { get; private set; }

    /// <summary>
    /// Total generation recorded since agreement start in kWh.
    /// Example: 125000.50 for 125,000.5 kWh lifetime generation. Default: 0.00.
    /// Cumulative metric for system performance tracking.
    /// </summary>
    public decimal LifetimeGeneration { get; private set; }

    /// <summary>
    /// Number of solar panels or wind turbines installed.
    /// Example: 25 for 25-panel solar array. Null if not applicable.
    /// </summary>
    public int? NumberOfPanels { get; private set; }

    /// <summary>
    /// Inverter manufacturer name.
    /// Example: "Enphase", "SolarEdge". Max length: 100.
    /// </summary>
    public string? InverterManufacturer { get; private set; }

    /// <summary>
    /// Inverter model number.
    /// Example: "IQ8+", "SE7600H". Max length: 100.
    /// </summary>
    public string? InverterModel { get; private set; }

    /// <summary>
    /// Solar panel manufacturer name.
    /// Example: "LG", "SunPower". Max length: 100.
    /// </summary>
    public string? PanelManufacturer { get; private set; }

    /// <summary>
    /// Solar panel model number.
    /// Example: "NeON 2", "X-Series". Max length: 100.
    /// </summary>
    public string? PanelModel { get; private set; }

    /// <summary>
    /// Date of last system inspection.
    /// Example: 2025-08-15. Used for safety and compliance tracking.
    /// </summary>
    public DateTime? LastInspectionDate { get; private set; }

    /// <summary>
    /// Date when next inspection is due.
    /// Example: 2026-08-15 for annual inspections. Calculated from last inspection.
    /// </summary>
    public DateTime? NextInspectionDate { get; private set; }

    /// <summary>
    /// One-time interconnection fee paid at system commissioning.
    /// Example: 500.00 for $500 interconnection fee. Default: 0.00.
    /// </summary>
    public decimal InterconnectionFee { get; private set; }

    /// <summary>
    /// Security deposit amount for system decommissioning.
    /// Example: 1000.00 for $1,000 deposit. Default: 0.00.
    /// Refunded upon proper system removal.
    /// </summary>
    public decimal DepositAmount { get; private set; }

    /// <summary>
    /// Whether the agreement is currently active.
    /// Default: true. False when suspended or terminated.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Date when the agreement was terminated.
    /// Example: 2025-12-31. Null if still active.
    /// </summary>
    public DateTime? TerminationDate { get; private set; }

    /// <summary>
    /// Reason for termination if agreement was terminated.
    /// Example: "System decommissioned", "Member moved". Max length: 500.
    /// </summary>
    public string? TerminationReason { get; private set; }

    // Parameterless constructor for EF Core
    private InterconnectionAgreement()
    {
        AgreementNumber = string.Empty;
        GenerationType = string.Empty;
        AgreementStatus = "Active";
    }

    private InterconnectionAgreement(string agreementNumber, DefaultIdType memberId,
        string generationType, DateTime effectiveDate, decimal installedCapacityKW,
        decimal netMeteringRate, decimal excessGenerationRate,
        decimal monthlyServiceCharge = 0, DateTime? expirationDate = null,
        decimal? annualGenerationLimit = null, int? numberOfPanels = null,
        string? inverterManufacturer = null, string? inverterModel = null,
        string? panelManufacturer = null, string? panelModel = null,
        decimal interconnectionFee = 0, decimal depositAmount = 0,
        string? description = null, string? notes = null)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(agreementNumber))
            throw new ArgumentException("Agreement number is required", nameof(agreementNumber));

        if (agreementNumber.Length > MaxAgreementNumberLength)
            throw new ArgumentException($"Agreement number cannot exceed {MaxAgreementNumberLength} characters", nameof(agreementNumber));

        if (string.IsNullOrWhiteSpace(generationType))
            throw new ArgumentException("Generation type is required", nameof(generationType));

        if (generationType.Length > MaxGenerationTypeLength)
            throw new ArgumentException($"Generation type cannot exceed {MaxGenerationTypeLength} characters", nameof(generationType));

        if (installedCapacityKW <= 0)
            throw new ArgumentException("Installed capacity must be positive", nameof(installedCapacityKW));

        if (netMeteringRate < 0)
            throw new ArgumentException("Net metering rate cannot be negative", nameof(netMeteringRate));

        if (excessGenerationRate < 0)
            throw new ArgumentException("Excess generation rate cannot be negative", nameof(excessGenerationRate));

        if (monthlyServiceCharge < 0)
            throw new ArgumentException("Monthly service charge cannot be negative", nameof(monthlyServiceCharge));

        if (interconnectionFee < 0)
            throw new ArgumentException("Interconnection fee cannot be negative", nameof(interconnectionFee));

        if (depositAmount < 0)
            throw new ArgumentException("Deposit amount cannot be negative", nameof(depositAmount));

        AgreementNumber = agreementNumber.Trim();
        Name = agreementNumber.Trim(); // For AuditableEntity compatibility
        MemberId = memberId;
        GenerationType = generationType.Trim();
        AgreementStatus = "Active";
        EffectiveDate = effectiveDate;
        ExpirationDate = expirationDate;
        InstalledCapacityKW = installedCapacityKW;
        NetMeteringRate = netMeteringRate;
        ExcessGenerationRate = excessGenerationRate;
        MonthlyServiceCharge = monthlyServiceCharge;
        CurrentCreditBalance = 0m;
        AnnualGenerationLimit = annualGenerationLimit;
        YearToDateGeneration = 0m;
        LifetimeGeneration = 0m;
        NumberOfPanels = numberOfPanels;
        InverterManufacturer = inverterManufacturer?.Trim();
        InverterModel = inverterModel?.Trim();
        PanelManufacturer = panelManufacturer?.Trim();
        PanelModel = panelModel?.Trim();
        InterconnectionFee = interconnectionFee;
        DepositAmount = depositAmount;
        IsActive = true;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new InterconnectionAgreementCreated(Id, AgreementNumber, MemberId, GenerationType, InstalledCapacityKW, EffectiveDate, Description, Notes));
    }

    /// <summary>
    /// Factory method to create a new interconnection agreement with validation.
    /// </summary>
    public static InterconnectionAgreement Create(string agreementNumber, DefaultIdType memberId,
        string generationType, DateTime effectiveDate, decimal installedCapacityKW,
        decimal netMeteringRate, decimal excessGenerationRate,
        decimal monthlyServiceCharge = 0, DateTime? expirationDate = null,
        decimal? annualGenerationLimit = null, int? numberOfPanels = null,
        string? inverterManufacturer = null, string? inverterModel = null,
        string? panelManufacturer = null, string? panelModel = null,
        decimal interconnectionFee = 0, decimal depositAmount = 0,
        string? description = null, string? notes = null)
    {
        return new InterconnectionAgreement(agreementNumber, memberId, generationType,
            effectiveDate, installedCapacityKW, netMeteringRate, excessGenerationRate,
            monthlyServiceCharge, expirationDate, annualGenerationLimit, numberOfPanels,
            inverterManufacturer, inverterModel, panelManufacturer, panelModel,
            interconnectionFee, depositAmount, description, notes);
    }

    /// <summary>
    /// Update agreement details.
    /// </summary>
    public InterconnectionAgreement Update(decimal? netMeteringRate = null,
        decimal? excessGenerationRate = null, decimal? monthlyServiceCharge = null,
        decimal? annualGenerationLimit = null, string? description = null, string? notes = null)
    {
        if (AgreementStatus == "Terminated")
            throw new InvalidOperationException("Cannot modify terminated agreement");

        bool isUpdated = false;

        if (netMeteringRate.HasValue && NetMeteringRate != netMeteringRate.Value)
        {
            if (netMeteringRate.Value < 0)
                throw new ArgumentException("Net metering rate cannot be negative");
            NetMeteringRate = netMeteringRate.Value;
            isUpdated = true;
        }

        if (excessGenerationRate.HasValue && ExcessGenerationRate != excessGenerationRate.Value)
        {
            if (excessGenerationRate.Value < 0)
                throw new ArgumentException("Excess generation rate cannot be negative");
            ExcessGenerationRate = excessGenerationRate.Value;
            isUpdated = true;
        }

        if (monthlyServiceCharge.HasValue && MonthlyServiceCharge != monthlyServiceCharge.Value)
        {
            if (monthlyServiceCharge.Value < 0)
                throw new ArgumentException("Monthly service charge cannot be negative");
            MonthlyServiceCharge = monthlyServiceCharge.Value;
            isUpdated = true;
        }

        if (annualGenerationLimit != AnnualGenerationLimit)
        {
            AnnualGenerationLimit = annualGenerationLimit;
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
            QueueDomainEvent(new InterconnectionAgreementUpdated(Id, AgreementNumber, Description, Notes));
        }

        return this;
    }

    /// <summary>
    /// Record generation from the system.
    /// </summary>
    public InterconnectionAgreement RecordGeneration(decimal generationKWh, DateTime recordDate)
    {
        if (generationKWh <= 0)
            throw new ArgumentException("Generation amount must be positive", nameof(generationKWh));

        if (!IsActive)
            throw new InvalidOperationException("Cannot record generation for inactive agreement");

        // Check annual limit
        if (AnnualGenerationLimit.HasValue && YearToDateGeneration + generationKWh > AnnualGenerationLimit.Value)
        {
            throw new InvalidOperationException($"Generation exceeds annual limit of {AnnualGenerationLimit.Value:N2} kWh");
        }

        YearToDateGeneration += generationKWh;
        LifetimeGeneration += generationKWh;

        QueueDomainEvent(new GenerationRecorded(Id, AgreementNumber, generationKWh, YearToDateGeneration, LifetimeGeneration, recordDate));
        return this;
    }

    /// <summary>
    /// Apply net metering credit to balance.
    /// </summary>
    public InterconnectionAgreement ApplyCredit(decimal creditAmount)
    {
        if (creditAmount <= 0)
            throw new ArgumentException("Credit amount must be positive", nameof(creditAmount));

        CurrentCreditBalance += creditAmount;

        QueueDomainEvent(new CreditApplied(Id, AgreementNumber, creditAmount, CurrentCreditBalance));
        return this;
    }

    /// <summary>
    /// Use credit balance (e.g., for billing offset).
    /// </summary>
    public InterconnectionAgreement UseCredit(decimal creditAmount)
    {
        if (creditAmount <= 0)
            throw new ArgumentException("Credit amount must be positive", nameof(creditAmount));

        if (creditAmount > CurrentCreditBalance)
            throw new InvalidOperationException($"Credit amount {creditAmount:N2} exceeds available balance {CurrentCreditBalance:N2}");

        CurrentCreditBalance -= creditAmount;

        QueueDomainEvent(new CreditUsed(Id, AgreementNumber, creditAmount, CurrentCreditBalance));
        return this;
    }

    /// <summary>
    /// Reset year-to-date generation (typically done annually).
    /// </summary>
    public InterconnectionAgreement ResetYearToDateGeneration()
    {
        YearToDateGeneration = 0m;
        return this;
    }

    /// <summary>
    /// Record system inspection.
    /// </summary>
    public InterconnectionAgreement RecordInspection(DateTime inspectionDate, int monthsUntilNextInspection = 12)
    {
        LastInspectionDate = inspectionDate;
        NextInspectionDate = inspectionDate.AddMonths(monthsUntilNextInspection);

        QueueDomainEvent(new InspectionRecorded(Id, AgreementNumber, inspectionDate, NextInspectionDate));
        return this;
    }

    /// <summary>
    /// Suspend the agreement.
    /// </summary>
    public InterconnectionAgreement Suspend(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Suspension reason is required", nameof(reason));

        if (AgreementStatus == "Terminated")
            throw new InvalidOperationException("Cannot suspend terminated agreement");

        AgreementStatus = "Suspended";
        IsActive = false;

        QueueDomainEvent(new InterconnectionAgreementSuspended(Id, AgreementNumber, reason));
        return this;
    }

    /// <summary>
    /// Activate a suspended agreement.
    /// </summary>
    public InterconnectionAgreement Activate()
    {
        if (AgreementStatus == "Terminated")
            throw new InvalidOperationException("Cannot activate terminated agreement");

        AgreementStatus = "Active";
        IsActive = true;

        QueueDomainEvent(new InterconnectionAgreementActivated(Id, AgreementNumber));
        return this;
    }

    /// <summary>
    /// Terminate the agreement.
    /// </summary>
    public InterconnectionAgreement Terminate(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Termination reason is required", nameof(reason));

        if (AgreementStatus == "Terminated")
            throw new InvalidOperationException("Agreement is already terminated");

        AgreementStatus = "Terminated";
        IsActive = false;
        TerminationDate = DateTime.UtcNow;
        TerminationReason = reason.Trim();

        QueueDomainEvent(new InterconnectionAgreementTerminated(Id, AgreementNumber, TerminationReason, TerminationDate.Value));
        return this;
    }

    /// <summary>
    /// Percentage of annual limit consumed.
    /// </summary>
    public decimal? AnnualLimitPercentageUsed => AnnualGenerationLimit.HasValue && AnnualGenerationLimit.Value > 0
        ? (YearToDateGeneration / AnnualGenerationLimit.Value) * 100
        : null;

    /// <summary>
    /// Whether annual generation limit has been reached.
    /// </summary>
    public bool IsAnnualLimitReached => AnnualGenerationLimit.HasValue && YearToDateGeneration >= AnnualGenerationLimit.Value;

    /// <summary>
    /// Whether system inspection is overdue.
    /// </summary>
    public bool IsInspectionOverdue => NextInspectionDate.HasValue && DateTime.UtcNow.Date > NextInspectionDate.Value.Date;

    /// <summary>
    /// Average monthly generation based on lifetime statistics.
    /// </summary>
    public decimal AverageMonthlyGeneration
    {
        get
        {
            if (LifetimeGeneration == 0) return 0;
            var monthsActive = Math.Max(1, (DateTime.UtcNow - EffectiveDate).Days / 30);
            return LifetimeGeneration / monthsActive;
        }
    }
}

