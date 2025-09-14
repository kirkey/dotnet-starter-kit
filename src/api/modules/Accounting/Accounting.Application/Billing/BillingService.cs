namespace Accounting.Application.Billing;

public class BillingService : IBillingService
{
    public InvoiceDraft CalculateInvoiceDraft(ConsumptionData consumption, RateSchedule rateSchedule)
    {
        if (consumption == null) throw new ArgumentNullException(nameof(consumption));
        if (rateSchedule == null) throw new ArgumentNullException(nameof(rateSchedule));

        var usage = consumption.KWhUsed;
        var lines = new List<InvoiceLineDto>();
        decimal usageCharge = 0m;

        // If the schedule has tiers, apply them in order
        var tiers = rateSchedule.Tiers.OrderBy(t => t.TierOrder).ToList();
        if (tiers.Count > 0)
        {
            decimal remaining = usage;
            foreach (var tier in tiers)
            {
                if (remaining <= 0) break;

                decimal qty = tier.UpToKwh == 0 ? remaining : Math.Min(remaining, tier.UpToKwh);
                decimal charge = qty * tier.RatePerKwh;
                lines.Add(new InvoiceLineDto($"Energy - Tier {tier.TierOrder}", qty, tier.RatePerKwh, charge));
                usageCharge += charge;
                remaining -= qty;
            }

            // If after tiers there's still remaining and last tier UpToKwh was >0, apply last tier rate if last tier UpToKwh ==0 covers unlimited; otherwise assume no further tiers.
            // (Handled above by UpToKwh==0 meaning unlimited.)
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

