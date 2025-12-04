using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for share products.
/// </summary>
internal static class ShareProductSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.ShareProducts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var products = new (string Code, string Name, string Desc, decimal Nominal, decimal Current, int MinShares, int? MaxShares, bool Transfer, bool Redeem, bool Dividends)[]
        {
            ("ORD-SHARE", "Ordinary Shares", "Standard membership shares", 10, 10, 5, 1000, false, true, true),
            ("PREF-SHARE", "Preference Shares", "Preferred dividend shares", 50, 55, 10, 500, true, true, true),
            ("FOUNDER-SHARE", "Founder Shares", "Special shares for founding members", 100, 120, 1, 100, false, false, true),
            ("STAFF-SHARE", "Staff Shares", "Shares for employees", 25, 25, 5, 200, false, true, true),
            ("YOUTH-SHARE", "Youth Shares", "Affordable shares for young members", 5, 5, 2, 100, false, true, true),
            ("PREMIUM-SHARE", "Premium Shares", "High-value investment shares", 100, 115, 10, 1000, true, true, true),
            ("RESERVE-SHARE", "Reserve Shares", "Institution reserve shares", 500, 500, 1, null, false, false, true),
            ("COMMUNITY-SHARE", "Community Shares", "Community investment shares", 20, 22, 5, 500, true, true, true),
            ("GROWTH-SHARE", "Growth Shares", "Capital appreciation focused", 75, 85, 10, 500, true, true, false),
            ("DIVIDEND-SHARE", "Dividend Shares", "High dividend yield shares", 30, 32, 5, 1000, false, true, true),
        };

        for (int i = existingCount; i < products.Length; i++)
        {
            var p = products[i];
            if (await context.ShareProducts.AnyAsync(x => x.Code == p.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var product = ShareProduct.Create(
                code: p.Code,
                name: p.Name,
                description: p.Desc,
                nominalValue: p.Nominal,
                currentPrice: p.Current,
                minSharesForMembership: p.MinShares,
                maxSharesPerMember: p.MaxShares,
                allowTransfer: p.Transfer,
                allowRedemption: p.Redeem,
                paysDividends: p.Dividends);

            await context.ShareProducts.AddAsync(product, cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] seeded share products", tenant);
    }
}
