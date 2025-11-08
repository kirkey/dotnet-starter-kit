using Accounting.Domain.Events.PowerPurchaseAgreement;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a power purchase agreement (PPA) contract for buying or selling bulk electricity with wholesale energy suppliers or other utilities.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track wholesale power purchase contracts from generators and power marketers.
/// - Manage power sales agreements with neighboring utilities and wholesale customers.
/// - Calculate monthly purchase obligations and settlement amounts.
/// - Support cost allocation to member rate classes for rate-making.
/// - Enable forecasting and budgeting for purchased power expenses.
/// - Track contract performance and deviation from contracted amounts.
/// - Support regulatory reporting for wholesale power transactions (FERC, EIA).
/// - Manage contract amendments, renewals, and terminations.
/// 
/// Default values:
/// - ContractNumber: required unique identifier (example: "PPA-2025-001", "PURCHASE-SOLAR-01")
/// - CounterpartyName: required name of seller or buyer (example: "ABC Solar LLC", "XYZ Utility")
/// - ContractType: required type (example: "Purchase", "Sale", "Exchange")
/// - StartDate: required contract effective date (example: 2025-01-01)
/// - EndDate: required contract expiration date (example: 2030-12-31)
/// - EnergyPricePerKWh: base energy price (example: 0.0450 for $45/MWh)
/// - DemandChargePerKW: optional demand charge (example: 5.50 for $5.50/kW-month)
/// - MinimumPurchaseKWh: optional minimum take-or-pay (example: 1000000 for 1 GWh/month)
/// - Status: "Active" (new contracts start as active)
/// - MonthlySettlementAmount: 0.00 (updated from actual deliveries)
/// - YearToDateCost: 0.00 (accumulated cost for fiscal year)
/// - LifetimeCost: 0.00 (total cost since contract inception)
/// 
/// Business rules:
/// - ContractNumber must be unique within utility system
/// - EndDate must be after StartDate
/// - Energy and demand prices cannot be negative
/// - MinimumPurchase creates take-or-pay obligation (pay even if not taken)
/// - Status transitions: Draft → Active → Suspended → Expired → Terminated
/// - Settlement amounts posted monthly based on meter data
/// - Cost allocation required for rate-making and cost recovery
/// - Contract modifications require amendment tracking
/// - Renewable energy credits (RECs) may be bundled or unbundled
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.PowerPurchaseAgreement.PowerPurchaseAgreementCreated"/>
/// <seealso cref="Accounting.Domain.Events.PowerPurchaseAgreement.PowerPurchaseAgreementUpdated"/>
/// <seealso cref="Accounting.Domain.Events.PowerPurchaseAgreement.PowerPurchaseAgreementActivated"/>
/// <seealso cref="Accounting.Domain.Events.PowerPurchaseAgreement.PowerPurchaseAgreementSuspended"/>
/// <seealso cref="Accounting.Domain.Events.PowerPurchaseAgreement.PowerPurchaseAgreementTerminated"/>
/// <seealso cref="Accounting.Domain.Events.PowerPurchaseAgreement.PowerPurchaseAgreementSettled"/>
public class PowerPurchaseAgreement : AuditableEntity, IAggregateRoot
{
    private const int MaxContractNumberLength = 50;
    private const int MaxCounterpartyNameLength = 256;
    private const int MaxContractTypeLength = 32;
    private const int MaxStatusLength = 32;
    private const int MaxEnergySourceLength = 100;
    private const int MaxSettlementFrequencyLength = 32;
    private const int MaxTerminationReasonLength = 1000;
    private const int MaxDescriptionLength = 2048;
    private const int MaxNotesLength = 2048;

    /// <summary>
    /// Unique contract number or identifier.
    /// Example: "PPA-2025-001", "SOLAR-PPA-ABC". Max length: 50.
    /// </summary>
    public string ContractNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Name of the counterparty (seller or buyer).
    /// Example: "ABC Solar Farm LLC", "XYZ Municipal Utility". Max length: 256.
    /// </summary>
    public string CounterpartyName { get; private set; } = string.Empty;

    /// <summary>
    /// Type of power purchase agreement.
    /// Values: "Purchase", "Sale", "Exchange", "Tolling", "Capacity".
    /// Example: "Purchase" for buying power, "Sale" for selling excess. Max length: 32.
    /// </summary>
    public string ContractType { get; private set; } = string.Empty;

    /// <summary>
    /// Contract effective start date.
    /// Example: 2025-01-01. Used for contract term and anniversary calculations.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Contract expiration or end date.
    /// Example: 2030-12-31 for 5-year contract. Must be after StartDate.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// Energy price per kilowatt-hour.
    /// Example: 0.0450 for $45/MWh or 4.5 cents/kWh. Cannot be negative.
    /// </summary>
    public decimal EnergyPricePerKWh { get; private set; }

    /// <summary>
    /// Optional demand charge per kilowatt per month.
    /// Example: 5.50 for $5.50/kW-month capacity charge. Null if no demand component.
    /// </summary>
    public decimal? DemandChargePerKw { get; private set; }

    /// <summary>
    /// Optional minimum purchase obligation (take-or-pay) in kWh per settlement period.
    /// Example: 1000000 for 1 GWh/month minimum. Null if no minimum.
    /// </summary>
    public decimal? MinimumPurchaseKWh { get; private set; }

    /// <summary>
    /// Optional maximum purchase limit in kWh per settlement period.
    /// Example: 5000000 for 5 GWh/month cap. Null if no maximum.
    /// </summary>
    public decimal? MaximumPurchaseKWh { get; private set; }

    /// <summary>
    /// Current contract status.
    /// Values: "Draft", "Active", "Suspended", "Expired", "Terminated".
    /// Default: "Active". Max length: 32.
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// Optional counterparty vendor identifier for accounts payable integration.
    /// Links to Vendor entity for payment processing.
    /// </summary>
    public DefaultIdType? VendorId { get; private set; }

    /// <summary>
    /// Settlement frequency for billing/payment.
    /// Values: "Monthly", "Quarterly", "Weekly", "Daily", "Hourly".
    /// Example: "Monthly" for standard monthly settlements. Max length: 32.
    /// </summary>
    public string SettlementFrequency { get; private set; } = string.Empty;

    /// <summary>
    /// Most recent monthly settlement amount.
    /// Example: 67500.00 for last month's power purchase cost. Default: 0.00.
    /// Updated with each settlement posting.
    /// </summary>
    public decimal MonthlySettlementAmount { get; private set; }

    /// <summary>
    /// Year-to-date accumulated cost for fiscal year.
    /// Example: 540000.00 for 8 months at ~$67,500/month. Default: 0.00.
    /// Reset at fiscal year end.
    /// </summary>
    public decimal YearToDateCost { get; private set; }

    /// <summary>
    /// Lifetime total cost since contract inception.
    /// Example: 2700000.00 for 40 months of purchases. Default: 0.00.
    /// Never reset, accumulates over contract life.
    /// </summary>
    public decimal LifetimeCost { get; private set; }

    /// <summary>
    /// Year-to-date energy delivered or purchased (in kWh).
    /// Example: 15000000.0 for 15 GWh year-to-date. Default: 0.00.
    /// Reset at fiscal year end.
    /// </summary>
    public decimal YearToDateEnergyKWh { get; private set; }

    /// <summary>
    /// Lifetime energy delivered since contract inception (in kWh).
    /// Example: 60000000.0 for 60 GWh total. Default: 0.00.
    /// Never reset.
    /// </summary>
    public decimal LifetimeEnergyKWh { get; private set; }

    /// <summary>
    /// Energy source or generation type.
    /// Example: "Solar", "Wind", "Natural Gas", "Hydro", "Mixed". Max length: 100.
    /// Important for renewable energy tracking and reporting.
    /// </summary>
    public string? EnergySource { get; private set; }

    /// <summary>
    /// Whether renewable energy credits (RECs) are included.
    /// Default: false. True if RECs bundled with energy, false if unbundled.
    /// </summary>
    public bool IncludesRenewableCredits { get; private set; }

    /// <summary>
    /// Contract capacity in megawatts (MW).
    /// Example: 25.0 for 25 MW solar farm. Null if not applicable.
    /// </summary>
    public decimal? ContractCapacityMw { get; private set; }

    /// <summary>
    /// Whether contract has take-or-pay minimum purchase obligation.
    /// Default: false. True if minimum purchase required regardless of usage.
    /// </summary>
    public bool IsTakeOrPay { get; private set; }

    /// <summary>
    /// Whether pricing includes escalation clauses.
    /// Default: false. True if prices adjust annually or periodically.
    /// </summary>
    public bool HasPriceEscalation { get; private set; }

    /// <summary>
    /// Annual price escalation percentage if applicable.
    /// Example: 0.025 for 2.5% annual increase. Null if no escalation.
    /// </summary>
    public decimal? EscalationRate { get; private set; }

    /// <summary>
    /// Next scheduled price escalation date.
    /// Example: 2026-01-01 for annual escalation. Null if no escalation.
    /// </summary>
    public DateTime? NextEscalationDate { get; private set; }

    /// <summary>
    /// Date contract was activated.
    /// Example: 2025-01-01. Null if still in draft.
    /// </summary>
    public DateTime? ActivationDate { get; private set; }

    /// <summary>
    /// Date contract was terminated, if applicable.
    /// Example: 2028-06-30 if terminated early. Null if still active.
    /// </summary>
    public DateTime? TerminationDate { get; private set; }

    /// <summary>
    /// Reason for early termination.
    /// Example: "Counterparty default", "Force majeure", "Contract buyout". Max length: 1000.
    /// </summary>
    public string? TerminationReason { get; private set; }

    /// <summary>
    /// Optional expense account for purchased power costs.
    /// Links to ChartOfAccount entity (typically USOA 555 - Purchased Power).
    /// </summary>
    public DefaultIdType? ExpenseAccountId { get; private set; }

    /// <summary>
    /// Optional accounting period for reporting.
    /// Links to AccountingPeriod entity for fiscal period tracking.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }

    // Parameterless constructor for EF Core
    private PowerPurchaseAgreement()
    {
        ContractNumber = string.Empty;
        CounterpartyName = string.Empty;
        ContractType = string.Empty;
        Status = "Active";
        SettlementFrequency = "Monthly";
    }

    private PowerPurchaseAgreement(string contractNumber, string counterpartyName,
        string contractType, DateTime startDate, DateTime endDate,
        decimal energyPricePerKWh, decimal? demandChargePerKw = null,
        decimal? minimumPurchaseKWh = null, decimal? maximumPurchaseKWh = null,
        DefaultIdType? vendorId = null, string settlementFrequency = "Monthly",
        string? energySource = null, bool includesRenewableCredits = false,
        decimal? contractCapacityMw = null, bool isTakeOrPay = false,
        bool hasPriceEscalation = false, decimal? escalationRate = null,
        DateTime? nextEscalationDate = null, DefaultIdType? expenseAccountId = null,
        DefaultIdType? periodId = null, string? description = null, string? notes = null)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(contractNumber))
            throw new ArgumentException("Contract number is required", nameof(contractNumber));

        if (contractNumber.Length > MaxContractNumberLength)
            throw new ArgumentException($"Contract number cannot exceed {MaxContractNumberLength} characters", nameof(contractNumber));

        if (string.IsNullOrWhiteSpace(counterpartyName))
            throw new ArgumentException("Counterparty name is required", nameof(counterpartyName));

        if (counterpartyName.Length > MaxCounterpartyNameLength)
            throw new ArgumentException($"Counterparty name cannot exceed {MaxCounterpartyNameLength} characters", nameof(counterpartyName));

        if (string.IsNullOrWhiteSpace(contractType))
            throw new ArgumentException("Contract type is required", nameof(contractType));

        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date", nameof(endDate));

        if (energyPricePerKWh < 0)
            throw new ArgumentException("Energy price cannot be negative", nameof(energyPricePerKWh));

        if (demandChargePerKw.HasValue && demandChargePerKw.Value < 0)
            throw new ArgumentException("Demand charge cannot be negative", nameof(demandChargePerKw));

        if (minimumPurchaseKWh.HasValue && minimumPurchaseKWh.Value < 0)
            throw new ArgumentException("Minimum purchase cannot be negative", nameof(minimumPurchaseKWh));

        if (maximumPurchaseKWh.HasValue && maximumPurchaseKWh.Value < 0)
            throw new ArgumentException("Maximum purchase cannot be negative", nameof(maximumPurchaseKWh));

        if (minimumPurchaseKWh.HasValue && maximumPurchaseKWh.HasValue && minimumPurchaseKWh.Value > maximumPurchaseKWh.Value)
            throw new ArgumentException("Minimum purchase cannot exceed maximum purchase");

        if (contractCapacityMw.HasValue && contractCapacityMw.Value <= 0)
            throw new ArgumentException("Contract capacity must be positive if specified", nameof(contractCapacityMw));

        if (escalationRate.HasValue && escalationRate.Value < 0)
            throw new ArgumentException("Escalation rate cannot be negative", nameof(escalationRate));

        ContractNumber = contractNumber.Trim();
        Name = contractNumber.Trim(); // For AuditableEntity compatibility
        CounterpartyName = counterpartyName.Trim();
        ContractType = contractType.Trim();
        StartDate = startDate;
        EndDate = endDate;
        EnergyPricePerKWh = energyPricePerKWh;
        DemandChargePerKw = demandChargePerKw;
        MinimumPurchaseKWh = minimumPurchaseKWh;
        MaximumPurchaseKWh = maximumPurchaseKWh;
        Status = "Active";
        VendorId = vendorId;
        SettlementFrequency = settlementFrequency.Trim();
        MonthlySettlementAmount = 0m;
        YearToDateCost = 0m;
        LifetimeCost = 0m;
        YearToDateEnergyKWh = 0m;
        LifetimeEnergyKWh = 0m;
        EnergySource = energySource?.Trim();
        IncludesRenewableCredits = includesRenewableCredits;
        ContractCapacityMw = contractCapacityMw;
        IsTakeOrPay = isTakeOrPay || minimumPurchaseKWh.HasValue;
        HasPriceEscalation = hasPriceEscalation;
        EscalationRate = escalationRate;
        NextEscalationDate = nextEscalationDate;
        ExpenseAccountId = expenseAccountId;
        PeriodId = periodId;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new PowerPurchaseAgreementCreated(Id, ContractNumber, CounterpartyName, ContractType, StartDate, EndDate, EnergyPricePerKWh, Description));
    }

    /// <summary>
    /// Factory method to create a new power purchase agreement with validation.
    /// </summary>
    public static PowerPurchaseAgreement Create(string contractNumber, string counterpartyName,
        string contractType, DateTime startDate, DateTime endDate,
        decimal energyPricePerKWh, decimal? demandChargePerKw = null,
        decimal? minimumPurchaseKWh = null, decimal? maximumPurchaseKWh = null,
        DefaultIdType? vendorId = null, string settlementFrequency = "Monthly",
        string? energySource = null, bool includesRenewableCredits = false,
        decimal? contractCapacityMw = null, bool isTakeOrPay = false,
        bool hasPriceEscalation = false, decimal? escalationRate = null,
        DateTime? nextEscalationDate = null, DefaultIdType? expenseAccountId = null,
        DefaultIdType? periodId = null, string? description = null, string? notes = null)
    {
        return new PowerPurchaseAgreement(contractNumber, counterpartyName, contractType,
            startDate, endDate, energyPricePerKWh, demandChargePerKw, minimumPurchaseKWh,
            maximumPurchaseKWh, vendorId, settlementFrequency, energySource,
            includesRenewableCredits, contractCapacityMw, isTakeOrPay, hasPriceEscalation,
            escalationRate, nextEscalationDate, expenseAccountId, periodId, description, notes);
    }

    /// <summary>
    /// Update contract details; not allowed when terminated.
    /// </summary>
    public PowerPurchaseAgreement Update(decimal? energyPricePerKWh = null,
        decimal? demandChargePerKw = null, decimal? minimumPurchaseKWh = null,
        decimal? maximumPurchaseKWh = null, DateTime? endDate = null,
        string? settlementFrequency = null, DefaultIdType? vendorId = null,
        DefaultIdType? expenseAccountId = null, string? description = null, string? notes = null)
    {
        if (Status == "Terminated")
            throw new InvalidOperationException("Cannot modify terminated contract");

        bool isUpdated = false;

        if (energyPricePerKWh.HasValue && EnergyPricePerKWh != energyPricePerKWh.Value)
        {
            if (energyPricePerKWh.Value < 0)
                throw new ArgumentException("Energy price cannot be negative");
            EnergyPricePerKWh = energyPricePerKWh.Value;
            isUpdated = true;
        }

        if (demandChargePerKw.HasValue && DemandChargePerKw != demandChargePerKw.Value)
        {
            if (demandChargePerKw.Value < 0)
                throw new ArgumentException("Demand charge cannot be negative");
            DemandChargePerKw = demandChargePerKw.Value;
            isUpdated = true;
        }

        if (minimumPurchaseKWh.HasValue && MinimumPurchaseKWh != minimumPurchaseKWh.Value)
        {
            if (minimumPurchaseKWh.Value < 0)
                throw new ArgumentException("Minimum purchase cannot be negative");
            MinimumPurchaseKWh = minimumPurchaseKWh.Value;
            IsTakeOrPay = minimumPurchaseKWh.Value > 0;
            isUpdated = true;
        }

        if (maximumPurchaseKWh.HasValue && MaximumPurchaseKWh != maximumPurchaseKWh.Value)
        {
            if (maximumPurchaseKWh.Value < 0)
                throw new ArgumentException("Maximum purchase cannot be negative");
            MaximumPurchaseKWh = maximumPurchaseKWh.Value;
            isUpdated = true;
        }

        if (endDate.HasValue && EndDate != endDate.Value)
        {
            if (endDate.Value <= StartDate)
                throw new ArgumentException("End date must be after start date");
            EndDate = endDate.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(settlementFrequency) && SettlementFrequency != settlementFrequency.Trim())
        {
            SettlementFrequency = settlementFrequency.Trim();
            isUpdated = true;
        }

        if (vendorId.HasValue && VendorId != vendorId.Value)
        {
            VendorId = vendorId.Value;
            isUpdated = true;
        }

        if (expenseAccountId.HasValue && ExpenseAccountId != expenseAccountId.Value)
        {
            ExpenseAccountId = expenseAccountId.Value;
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
            QueueDomainEvent(new PowerPurchaseAgreementUpdated(Id, ContractNumber, CounterpartyName, ContractType, EnergyPricePerKWh, Description));
        }

        return this;
    }

    /// <summary>
    /// Activate the contract.
    /// </summary>
    public PowerPurchaseAgreement Activate()
    {
        if (Status == "Active")
            throw new InvalidOperationException("Contract is already active");

        Status = "Active";
        ActivationDate = DateTime.UtcNow;

        QueueDomainEvent(new PowerPurchaseAgreementActivated(Id, ContractNumber, CounterpartyName, ActivationDate.Value));
        return this;
    }

    /// <summary>
    /// Suspend the contract temporarily.
    /// </summary>
    public PowerPurchaseAgreement Suspend(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Suspension reason is required", nameof(reason));

        if (Status != "Active")
            throw new InvalidOperationException("Can only suspend active contracts");

        Status = "Suspended";
        Notes = $"{Notes}\n\nSuspended: {reason.Trim()}".Trim();

        QueueDomainEvent(new PowerPurchaseAgreementSuspended(Id, ContractNumber, CounterpartyName, reason));
        return this;
    }

    /// <summary>
    /// Reactivate a suspended contract.
    /// </summary>
    public PowerPurchaseAgreement Reactivate()
    {
        if (Status != "Suspended")
            throw new InvalidOperationException("Can only reactivate suspended contracts");

        Status = "Active";

        QueueDomainEvent(new PowerPurchaseAgreementReactivated(Id, ContractNumber, CounterpartyName));
        return this;
    }

    /// <summary>
    /// Terminate the contract permanently.
    /// </summary>
    public PowerPurchaseAgreement Terminate(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Termination reason is required", nameof(reason));

        if (Status == "Terminated")
            throw new InvalidOperationException("Contract is already terminated");

        Status = "Terminated";
        TerminationDate = DateTime.UtcNow;
        TerminationReason = reason.Trim();

        QueueDomainEvent(new PowerPurchaseAgreementTerminated(Id, ContractNumber, CounterpartyName, TerminationDate.Value, reason));
        return this;
    }

    /// <summary>
    /// Record a settlement for energy delivered/purchased.
    /// </summary>
    public PowerPurchaseAgreement RecordSettlement(decimal energyKWh, decimal settlementAmount, DateTime settlementDate)
    {
        if (Status != "Active")
            throw new InvalidOperationException("Can only record settlements for active contracts");

        if (energyKWh < 0)
            throw new ArgumentException("Energy amount cannot be negative", nameof(energyKWh));

        if (settlementAmount < 0)
            throw new ArgumentException("Settlement amount cannot be negative", nameof(settlementAmount));

        MonthlySettlementAmount = settlementAmount;
        YearToDateCost += settlementAmount;
        LifetimeCost += settlementAmount;
        YearToDateEnergyKWh += energyKWh;
        LifetimeEnergyKWh += energyKWh;

        QueueDomainEvent(new PowerPurchaseAgreementSettled(Id, ContractNumber, CounterpartyName, energyKWh, settlementAmount, settlementDate));
        return this;
    }

    /// <summary>
    /// Apply price escalation according to contract terms.
    /// </summary>
    public PowerPurchaseAgreement ApplyPriceEscalation()
    {
        if (!HasPriceEscalation || !EscalationRate.HasValue)
            throw new InvalidOperationException("Contract does not have price escalation");

        if (!NextEscalationDate.HasValue || DateTime.UtcNow.Date < NextEscalationDate.Value.Date)
            throw new InvalidOperationException("Not yet time for price escalation");

        decimal oldPrice = EnergyPricePerKWh;
        EnergyPricePerKWh = EnergyPricePerKWh * (1 + EscalationRate.Value);

        if (DemandChargePerKw.HasValue)
        {
            DemandChargePerKw = DemandChargePerKw.Value * (1 + EscalationRate.Value);
        }

        // Set next escalation date (typically one year forward)
        NextEscalationDate = NextEscalationDate.Value.AddYears(1);

        QueueDomainEvent(new PowerPurchaseAgreementPriceEscalated(Id, ContractNumber, oldPrice, EnergyPricePerKWh, EscalationRate.Value));
        return this;
    }

    /// <summary>
    /// Reset year-to-date totals at fiscal year end.
    /// </summary>
    public PowerPurchaseAgreement ResetYearToDateTotals()
    {
        YearToDateCost = 0m;
        YearToDateEnergyKWh = 0m;
        return this;
    }

    /// <summary>
    /// Mark contract as expired if past end date.
    /// </summary>
    public PowerPurchaseAgreement MarkExpired()
    {
        if (Status == "Terminated")
            throw new InvalidOperationException("Cannot mark terminated contract as expired");

        if (DateTime.UtcNow.Date <= EndDate.Date)
            throw new InvalidOperationException("Cannot mark contract as expired before end date");

        Status = "Expired";
        QueueDomainEvent(new PowerPurchaseAgreementExpired(Id, ContractNumber, CounterpartyName, EndDate));
        return this;
    }

    /// <summary>
    /// Whether the contract is currently active.
    /// </summary>
    public bool IsActive => Status == "Active";

    /// <summary>
    /// Whether the contract has expired.
    /// </summary>
    public bool IsExpired => DateTime.UtcNow.Date > EndDate.Date || Status == "Expired";

    /// <summary>
    /// Number of days until contract expiration.
    /// </summary>
    public int DaysUntilExpiration => Math.Max(0, (EndDate.Date - DateTime.UtcNow.Date).Days);

    /// <summary>
    /// Average cost per kWh based on lifetime totals.
    /// </summary>
    public decimal AverageCostPerKWh => LifetimeEnergyKWh > 0 ? LifetimeCost / LifetimeEnergyKWh : 0m;
}

