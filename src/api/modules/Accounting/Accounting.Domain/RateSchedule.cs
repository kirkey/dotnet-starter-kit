using Accounting.Domain.Events.RateSchedule;

namespace Accounting.Domain;

public class RateSchedule : AuditableEntity, IAggregateRoot
{
    public string RateCode { get; private set; }
    public string RateName { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public decimal EnergyRatePerKwh { get; private set; } // cents or currency per kWh
    public decimal? DemandRatePerKw { get; private set; } // for demand charges
    public decimal FixedMonthlyCharge { get; private set; }
    public bool IsTimeOfUse { get; private set; }

    private readonly List<RateTier> _tiers = new();
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

    public static RateSchedule Create(string rateCode, string rateName, DateTime effectiveDate, decimal energyRatePerKwh, decimal fixedMonthlyCharge, bool isTimeOfUse = false, decimal? demandRatePerKw = null, DateTime? expirationDate = null, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(rateCode)) throw new ArgumentException("Rate code is required.");
        if (string.IsNullOrWhiteSpace(rateName)) throw new ArgumentException("Rate name is required.");
        if (energyRatePerKwh < 0) throw new ArgumentException("Energy rate must be non-negative.");
        if (fixedMonthlyCharge < 0) throw new ArgumentException("Fixed monthly charge must be non-negative.");

        return new RateSchedule(rateCode, rateName, effectiveDate, energyRatePerKwh, fixedMonthlyCharge, isTimeOfUse, demandRatePerKw, expirationDate, description);
    }

    public RateSchedule Update(string? rateName = null, DateTime? effectiveDate = null, DateTime? expirationDate = null, decimal? energyRatePerKwh = null, decimal? demandRatePerKw = null, decimal? fixedMonthlyCharge = null, bool? isTimeOfUse = null, string? description = null)
    {
        bool isUpdated = false;
        if (!string.IsNullOrWhiteSpace(rateName) && RateName != rateName.Trim()) { RateName = rateName.Trim(); isUpdated = true; }
        if (effectiveDate.HasValue && EffectiveDate != effectiveDate.Value) { EffectiveDate = effectiveDate.Value; isUpdated = true; }
        if (expirationDate.HasValue && ExpirationDate != expirationDate) { ExpirationDate = expirationDate; isUpdated = true; }
        if (energyRatePerKwh.HasValue && EnergyRatePerKwh != energyRatePerKwh.Value) { EnergyRatePerKwh = energyRatePerKwh.Value; isUpdated = true; }
        if (demandRatePerKw.HasValue && DemandRatePerKw != demandRatePerKw.Value) { DemandRatePerKw = demandRatePerKw; isUpdated = true; }
        if (fixedMonthlyCharge.HasValue && FixedMonthlyCharge != fixedMonthlyCharge.Value) { FixedMonthlyCharge = fixedMonthlyCharge.Value; isUpdated = true; }
        if (isTimeOfUse.HasValue && IsTimeOfUse != isTimeOfUse.Value) { IsTimeOfUse = isTimeOfUse.Value; isUpdated = true; }
        if (description != Description) { Description = description?.Trim(); isUpdated = true; }

        if (isUpdated) QueueDomainEvent(new RateScheduleUpdated(this));
        return this;
    }

    public RateSchedule AddTier(int tierOrder, decimal upToKwh, decimal ratePerKwh)
    {
        var tier = RateTier.Create(Id, tierOrder, upToKwh, ratePerKwh);
        _tiers.Add(tier);
        QueueDomainEvent(new RateTierAdded(Id, tier.Id, tierOrder, upToKwh, ratePerKwh));
        return this;
    }
}

public class RateTier : BaseEntity
{
    public DefaultIdType RateScheduleId { get; private set; }
    public int TierOrder { get; private set; }
    public decimal UpToKwh { get; private set; } // 0 means unlimited
    public decimal RatePerKwh { get; private set; }

    private RateTier() { }

    private RateTier(DefaultIdType rateScheduleId, int tierOrder, decimal upToKwh, decimal ratePerKwh)
    {
        RateScheduleId = rateScheduleId;
        TierOrder = tierOrder;
        UpToKwh = upToKwh;
        RatePerKwh = ratePerKwh;
    }

    public static RateTier Create(DefaultIdType rateScheduleId, int tierOrder, decimal upToKwh, decimal ratePerKwh)
    {
        if (tierOrder <= 0) throw new ArgumentException("Tier order must be positive.");
        if (upToKwh < 0) throw new ArgumentException("UpToKwh must be non-negative.");
        if (ratePerKwh < 0) throw new ArgumentException("Rate per kWh must be non-negative.");

        return new RateTier(rateScheduleId, tierOrder, upToKwh, ratePerKwh);
    }
}
