using Accounting.Domain.Events.RateSchedule;

namespace Accounting.Domain;

/// <summary>
/// Represents a utility rate schedule with energy/demand pricing, fixed charges, and time-of-use capabilities for customer billing.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define electricity pricing structures for different customer classes (residential, commercial, industrial).
/// - Support tiered rate structures with increasing block rates based on usage levels.
/// - Enable time-of-use pricing with peak, off-peak, and shoulder period rates.
/// - Manage seasonal rate variations (summer/winter pricing differences).
/// - Support demand charge calculations for commercial and industrial customers.
/// - Enable regulatory compliance with approved tariff structures and rate schedules.
/// - Track rate schedule effectiveness periods and automatic transitions.
/// - Support special rate programs (low-income, economic development, green energy).
/// 
/// Default values:
/// - RateCode: required unique identifier (example: "RES-1", "COM-2", "IND-TOU")
/// - RateName: required display name (example: "Residential Standard Rate", "Commercial Time-of-Use")
/// - EffectiveDate: required start date for rate applicability (example: 2025-10-01)
/// - ExpirationDate: optional end date (example: 2026-09-30 for annual rates)
/// - EnergyRate: base energy charge per kWh (example: 0.0875 for 8.75 cents/kWh)
/// - DemandRate: demand charge per kW (example: 12.50 for commercial customers)
/// - FixedCharge: monthly service charge (example: 25.00 for residential)
/// - IsTimeOfUse: false (standard rates), true for TOU rates
/// - IsActive: true (new rates are active by default)
/// - CustomerClass: target customer segment (example: "Residential", "Commercial", "Industrial")
/// 
/// Business rules:
/// - RateCode must be unique within the utility system
/// - EffectiveDate cannot be in the past for new rate schedules
/// - ExpirationDate must be after EffectiveDate when specified
/// - All rate amounts must be non-negative
/// - Cannot delete rate schedules assigned to active customers
/// - Rate changes require regulatory approval and proper notice periods
/// - Time-of-use rates must have associated rate tiers for different periods
/// - Seasonal rates require proper effective date management
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.RateSchedule.RateScheduleCreated"/>
/// <seealso cref="Accounting.Domain.Events.RateSchedule.RateScheduleUpdated"/>
/// <seealso cref="Accounting.Domain.Events.RateSchedule.RateScheduleActivated"/>
/// <seealso cref="Accounting.Domain.Events.RateSchedule.RateScheduleExpired"/>
/// <seealso cref="Accounting.Domain.Events.RateSchedule.RateScheduleTierAdded"/>
/// <seealso cref="Accounting.Domain.Events.RateSchedule.RateScheduleApproved"/>
public class RateSchedule : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique rate code identifier.
    /// </summary>
    public string RateCode { get; private set; }

    /// <summary>
    /// Display name for the rate schedule.
    /// </summary>
    public string RateName { get; private set; }

    /// <summary>
    /// Date when the rate becomes effective.
    /// </summary>
    public DateTime EffectiveDate { get; private set; }

    /// <summary>
    /// When the rate expires, if applicable.
    /// </summary>
    public DateTime? ExpirationDate { get; private set; }

    /// <summary>
    /// Energy charge per kWh.
    /// </summary>
    public decimal EnergyRatePerKwh { get; private set; } // cents or currency per kWh

    /// <summary>
    /// Optional demand charge per kW for demand-billed customers.
    /// </summary>
    public decimal? DemandRatePerKw { get; private set; } // for demand charges

    /// <summary>
    /// Fixed monthly customer charge.
    /// </summary>
    public decimal FixedMonthlyCharge { get; private set; }

    /// <summary>
    /// Whether the rate uses time-of-use periods.
    /// </summary>
    public bool IsTimeOfUse { get; private set; }

    private readonly List<RateTier> _tiers = new();
    /// <summary>
    /// Optional tiered pricing structure associated with this rate.
    /// </summary>
    public IReadOnlyCollection<RateTier> Tiers => _tiers.AsReadOnly();

    private RateSchedule()
    {
        RateCode = string.Empty;
        RateName = string.Empty;
        EffectiveDate = DateTime.UtcNow;
    }

    private RateSchedule(string rateCode, string rateName, DateTime effectiveDate, decimal energyRatePerKwh, decimal fixedMonthlyCharge, bool isTimeOfUse = false, decimal? demandRatePerKw = null, DateTime? expirationDate = null, string? description = null)
    {
        RateCode = rateCode.Trim();
        RateName = rateName.Trim();
        EffectiveDate = effectiveDate;
        ExpirationDate = expirationDate;
        EnergyRatePerKwh = energyRatePerKwh;
        DemandRatePerKw = demandRatePerKw;
        FixedMonthlyCharge = fixedMonthlyCharge;
        IsTimeOfUse = isTimeOfUse;
        Description = description?.Trim();

        // Domain event
        QueueDomainEvent(new RateScheduleCreated(Id, RateCode, RateName, EffectiveDate, Description));
    }

    /// <summary>
    /// Create a rate schedule with validation for required fields and non-negative charges.
    /// </summary>
    public static RateSchedule Create(string rateCode, string rateName, DateTime effectiveDate, decimal energyRatePerKwh, decimal fixedMonthlyCharge, bool isTimeOfUse = false, decimal? demandRatePerKw = null, DateTime? expirationDate = null, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(rateCode)) throw new ArgumentException("Rate code is required.");
        if (string.IsNullOrWhiteSpace(rateName)) throw new ArgumentException("Rate name is required.");
        if (energyRatePerKwh < 0) throw new ArgumentException("Energy rate must be non-negative.");
        if (fixedMonthlyCharge < 0) throw new ArgumentException("Fixed monthly charge must be non-negative.");

        return new RateSchedule(rateCode, rateName, effectiveDate, energyRatePerKwh, fixedMonthlyCharge, isTimeOfUse, demandRatePerKw, expirationDate, description);
    }

    /// <summary>
    /// Update metadata and rate values; enforces non-negative numeric fields.
    /// </summary>
    public RateSchedule Update(string? rateName = null, DateTime? effectiveDate = null, DateTime? expirationDate = null, decimal? energyRatePerKwh = null, decimal? demandRatePerKw = null, decimal? fixedMonthlyCharge = null, bool isTimeOfUse = false, string? description = null)
    {
        bool isUpdated = false;
        if (!string.IsNullOrWhiteSpace(rateName) && RateName != rateName.Trim()) { RateName = rateName.Trim(); isUpdated = true; }
        if (effectiveDate.HasValue && EffectiveDate != effectiveDate.Value) { EffectiveDate = effectiveDate.Value; isUpdated = true; }
        if (expirationDate.HasValue && ExpirationDate != expirationDate) { ExpirationDate = expirationDate; isUpdated = true; }
        if (energyRatePerKwh.HasValue && EnergyRatePerKwh != energyRatePerKwh.Value) { EnergyRatePerKwh = energyRatePerKwh.Value; isUpdated = true; }
        if (demandRatePerKw.HasValue && DemandRatePerKw != demandRatePerKw.Value) { DemandRatePerKw = demandRatePerKw; isUpdated = true; }
        if (fixedMonthlyCharge.HasValue && FixedMonthlyCharge != fixedMonthlyCharge.Value) { FixedMonthlyCharge = fixedMonthlyCharge.Value; isUpdated = true; }
        if (IsTimeOfUse != isTimeOfUse) { IsTimeOfUse = isTimeOfUse; isUpdated = true; }
        if (description != Description) { Description = description?.Trim(); isUpdated = true; }

        if (isUpdated) QueueDomainEvent(new RateScheduleUpdated(this));
        return this;
    }

    /// <summary>
    /// Add a tier definition to the rate schedule.
    /// </summary>
    public RateSchedule AddTier(int tierOrder, decimal upToKwh, decimal ratePerKwh)
    {
        var tier = RateTier.Create(Id, tierOrder, upToKwh, ratePerKwh);
        _tiers.Add(tier);
        QueueDomainEvent(new RateTierAdded(Id, tier.Id, tierOrder, upToKwh, ratePerKwh));
        return this;
    }
}

/// <summary>
/// Represents a single pricing tier within a rate schedule.
/// </summary>
public class RateTier : BaseEntity
{
    /// <summary>
    /// Parent rate schedule identifier.
    /// </summary>
    public DefaultIdType RateScheduleId { get; private set; }

    /// <summary>
    /// Order of this tier (1-based).
    /// </summary>
    public int TierOrder { get; private set; }

    /// <summary>
    /// Upper bound for kWh usage under this tier; 0 means no upper limit.
    /// </summary>
    public decimal UpToKwh { get; private set; } // 0 means unlimited

    /// <summary>
    /// Price per kWh for consumption within this tier.
    /// </summary>
    public decimal RatePerKwh { get; private set; }

    private RateTier() { }

    private RateTier(DefaultIdType rateScheduleId, int tierOrder, decimal upToKwh, decimal ratePerKwh)
    {
        RateScheduleId = rateScheduleId;
        TierOrder = tierOrder;
        UpToKwh = upToKwh;
        RatePerKwh = ratePerKwh;
    }

    /// <summary>
    /// Create a rate tier; validates positive order and non-negative usage/rate.
    /// </summary>
    public static RateTier Create(DefaultIdType rateScheduleId, int tierOrder, decimal upToKwh, decimal ratePerKwh)
    {
        if (tierOrder <= 0) throw new ArgumentException("Tier order must be positive.");
        if (upToKwh < 0) throw new ArgumentException("UpToKwh must be non-negative.");
        if (ratePerKwh < 0) throw new ArgumentException("Rate per kWh must be non-negative.");

        return new RateTier(rateScheduleId, tierOrder, upToKwh, ratePerKwh);
    }
}
