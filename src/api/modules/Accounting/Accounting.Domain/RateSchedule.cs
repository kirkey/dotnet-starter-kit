using Accounting.Domain.Events.RateSchedule;

namespace Accounting.Domain;

/// <summary>
/// Represents a utility rate schedule with energy/demand rates, fixed charges, and optional time-of-use structure.
/// </summary>
/// <remarks>
/// Tracks effective and expiration dates, and supports tiered pricing via <see cref="RateTier"/> entries.
/// Defaults: strings trimmed; numeric rates must be non-negative; <see cref="IsTimeOfUse"/> defaults to false.
/// </remarks>
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
