namespace Accounting.Application.Billing;

public class BillingService : IBillingService
{
    public InvoiceDraft CalculateInvoiceDraft(Domain.Consumption consumption, RateSchedule rateSchedule)
    {
        if (consumption == null) throw new ArgumentNullException(nameof(consumption));
        if (rateSchedule == null) throw new ArgumentNullException(nameof(rateSchedule));

        var usage = consumption.KWhUsed;
        if (usage < 0) throw new ArgumentException("Consumption KWhUsed cannot be negative", nameof(consumption));

        var lines = new List<InvoiceLineDto>();
        decimal usageCharge = 0m;

        // If the schedule has tiers, apply them in order (UpToKwh is treated as a cumulative cap; 0 means unlimited)
        var tiers = rateSchedule.Tiers.OrderBy(t => t.TierOrder).ToList();
        if (tiers.Count > 0)
        {
            // Validate tiers: UpToKwh must be non-negative and non-decreasing (except 0 meaning unlimited which should be last)
            decimal prevCap = 0m;
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                if (t.UpToKwh < 0) throw new ArgumentException($"Rate tier UpToKwh must be non-negative (tier {t.TierOrder})");
                if (t.UpToKwh != 0 && t.UpToKwh < prevCap)
                    throw new ArgumentException($"Rate tiers must have non-decreasing UpToKwh values (tier {t.TierOrder})");
                // If an unlimited tier (UpToKwh == 0) appears before the last tier, treat it as invalid
                if (t.UpToKwh == 0 && i != tiers.Count - 1)
                    throw new ArgumentException($"Unlimited tier (UpToKwh==0) must be the last tier (tier {t.TierOrder})");
                prevCap = t.UpToKwh == 0 ? prevCap : t.UpToKwh;
            }

            decimal remaining = usage;
            decimal lowerBound = 0m; // previous cap
            foreach (var tier in tiers)
            {
                if (remaining <= 0) break;

                decimal tierCap = tier.UpToKwh; // cumulative upper bound (0 = unlimited)
                decimal qtyForThisTier;

                if (tierCap == 0m)
                {
                    // unlimited tier: all remaining usage
                    qtyForThisTier = remaining;
                }
                else
                {
                    // allocate only the portion between lowerBound and tierCap
                    var availableInTier = tierCap - lowerBound;
                    if (availableInTier <= 0)
                    {
                        lowerBound = tierCap;
                        continue; // nothing to allocate in this tier
                    }

                    qtyForThisTier = Math.Min(remaining, availableInTier);
                }

                if (qtyForThisTier > 0)
                {
                    decimal charge = qtyForThisTier * tier.RatePerKwh;
                    lines.Add(new InvoiceLineDto($"Energy - Tier {tier.TierOrder}", qtyForThisTier, tier.RatePerKwh, charge));
                    usageCharge += charge;
                    remaining -= qtyForThisTier;
                }

                // advance lower bound if this tier had a finite cap
                if (tierCap != 0m)
                    lowerBound = tierCap;
            }

            // If after all tiers there is still remaining usage and no unlimited tier existed, apply the schedule's base EnergyRatePerKwh
            if (tiers.All(t => t.UpToKwh != 0m) && remaining > 0)
            {
                decimal charge = remaining * rateSchedule.EnergyRatePerKwh;
                lines.Add(new InvoiceLineDto("Energy - Overage", remaining, rateSchedule.EnergyRatePerKwh, charge));
                usageCharge += charge;
            }
        }
        else
        {
            // No tiers - apply flat energy rate
            var charge = usage * rateSchedule.EnergyRatePerKwh;
            lines.Add(new InvoiceLineDto("Energy", usage, rateSchedule.EnergyRatePerKwh, charge));
            usageCharge = charge;
        }

        // Fixed charge line
        if (rateSchedule.FixedMonthlyCharge > 0)
        {
            lines.Add(new InvoiceLineDto("Fixed Monthly Charge", 1, rateSchedule.FixedMonthlyCharge, rateSchedule.FixedMonthlyCharge));
        }

        var total = usageCharge + rateSchedule.FixedMonthlyCharge;

        return new InvoiceDraft(usageCharge, rateSchedule.FixedMonthlyCharge, total, lines);
    }
}
