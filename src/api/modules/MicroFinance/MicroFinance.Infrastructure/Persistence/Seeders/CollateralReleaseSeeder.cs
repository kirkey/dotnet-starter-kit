using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for collateral releases.
/// Creates collateral release records for settled loans.
/// </summary>
internal static class CollateralReleaseSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.CollateralReleases.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        // Get collaterals for paid-off loans
        var paidLoans = await context.Loans
            .Where(l => l.Status == Loan.StatusClosed)
            .Take(10)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!paidLoans.Any()) return;

        var collaterals = await context.LoanCollaterals
            .Where(c => paidLoans.Select(l => l.Id).Contains(c.LoanId))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!collaterals.Any()) return;

        var random = new Random(42);
        int releaseNumber = 25001;
        int releaseCount = 0;

        var releaseMethods = new[] { CollateralRelease.MethodInPerson, CollateralRelease.MethodCourier, CollateralRelease.MethodElectronic };

        foreach (var collateral in collaterals)
        {
            var requestDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(10, 60)));
            var releaseMethod = releaseMethods[random.Next(releaseMethods.Length)];
            var recipientName = "Member Owner";
            var recipientIdNumber = $"PSA-{random.Next(1000000000, 1999999999):D10}";
            var recipientContact = $"+639{random.Next(100000000, 999999999)}";

            var release = CollateralRelease.Create(
                collateralId: collateral.Id,
                loanId: collateral.LoanId,
                releaseReference: $"REL-{releaseNumber++:D6}",
                requestedById: Guid.NewGuid(),
                releaseMethod: releaseMethod);

            // Update with recipient contact info
            release.Update(recipientContact: recipientContact);

            // Most releases are processed
            var status = random.NextDouble();
            if (status < 0.7) // 70% released
            {
                release.Approve(Guid.NewGuid());
                // Release method sets recipient name/ID and DocumentsReturned = true
                release.Release(Guid.NewGuid(), recipientName, recipientIdNumber);
                if (collateral.CollateralType == "Vehicle" || collateral.CollateralType == "RealEstate")
                {
                    release.MarkRegistrationCleared();
                }
            }
            else if (status < 0.9) // 20% approved, pending release
            {
                release.Approve(Guid.NewGuid());
            }
            // 10% still pending

            await context.CollateralReleases.AddAsync(release, cancellationToken).ConfigureAwait(false);
            releaseCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} collateral releases", tenant, releaseCount);
    }
}
