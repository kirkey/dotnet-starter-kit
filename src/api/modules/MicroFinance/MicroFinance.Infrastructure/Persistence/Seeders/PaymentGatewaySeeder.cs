using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for payment gateway configurations.
/// Creates payment gateway integrations for various providers.
/// </summary>
internal static class PaymentGatewaySeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.PaymentGateways.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var gateways = new (string Name, string Provider, decimal FeePercent, decimal FeeFixed, decimal MinAmount, decimal MaxAmount, bool Cards, bool Mobile, bool Bank)[]
        {
            // Philippine payment providers
            ("GCash Integration", "GCash", 1.5m, 0m, 1m, 100000m, false, true, false),
            ("PayMaya Integration", "PayMaya", 1.5m, 0m, 1m, 100000m, true, true, false),
            ("Dragonpay", "Dragonpay", 1.0m, 15m, 100m, 500000m, false, false, true),
            ("Paymongo", "Paymongo", 2.5m, 0m, 100m, 100000m, true, true, true),
            
            // International providers (for OFW remittances)
            ("Wise", "Wise", 0.5m, 0m, 1000m, 1000000m, false, false, true),
            ("Remitly", "Remitly", 0m, 50m, 500m, 500000m, false, false, true),
            ("Western Union", "WesternUnion", 0m, 100m, 1000m, 200000m, false, false, true),
            
            // Banks
            ("Instapay", "Instapay", 0m, 0m, 1m, 50000m, false, false, true),
            ("PESONet", "PESONet", 0m, 50m, 1000m, 1000000m, false, false, true),
            ("UnionBank Online", "UnionBank", 0m, 0m, 100m, 200000m, false, false, true),
        };

        foreach (var gw in gateways)
        {
            var gateway = PaymentGateway.Create(
                name: gw.Name,
                provider: gw.Provider,
                transactionFeePercent: gw.FeePercent,
                transactionFeeFixed: gw.FeeFixed,
                minTransactionAmount: gw.MinAmount,
                maxTransactionAmount: gw.MaxAmount);

            gateway.Update(
                supportsRefunds: true,
                supportsRecurring: gw.Bank,
                supportsMobileWallet: gw.Mobile,
                supportsCardPayments: gw.Cards,
                supportsBankTransfer: gw.Bank);

            gateway.Activate();
            gateway.SetTestMode(false);

            await context.PaymentGateways.AddAsync(gateway, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} payment gateways", tenant, gateways.Length);
    }
}
