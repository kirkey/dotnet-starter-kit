using System;
using System.Linq;
using Accounting.Application.Billing;
using Accounting.Domain;
using Xunit;

namespace Accounting.Application.Tests;

public class BillingServiceTests
{
    [Fact]
    public void CalculateInvoiceDraft_WithTiers_AppliesTiersCorrectly()
    {
        var service = new BillingService();

        // Create a rate schedule with tiers
        var rs = RateSchedule.Create("RES1", "Residential", DateTime.UtcNow, 0.10m, 5.00m);
        rs.AddTier(1, 100m, 0.08m);
        rs.AddTier(2, 200m, 0.12m);
        rs.AddTier(3, 0m, 0.20m); // unlimited

        var consumption = ConsumptionData.Create(DefaultIdType.NewGuid(), DateTime.UtcNow, 350m, 0m, "2025-09");

        var draft = service.CalculateInvoiceDraft(consumption, rs);

        // Expected: Tier1=100@0.08=8.00, Tier2=200@0.12=24.00, Tier3=50@0.20=10.00 => usage=42.00
        var usageCharge = draft.UsageCharge;
        Assert.Equal(42.00m, usageCharge);
        Assert.Equal(5.00m, draft.FixedCharge);
        Assert.Equal(47.00m, draft.TotalAmount);

        var tierLines = draft.Lines.Where(l => l.Description.StartsWith("Energy - Tier")).ToList();
        Assert.Equal(3, tierLines.Count);
    }
}

