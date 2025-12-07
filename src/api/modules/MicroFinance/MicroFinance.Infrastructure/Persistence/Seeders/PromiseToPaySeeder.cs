using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for promise to pay records.
/// Creates promises made by delinquent borrowers.
/// </summary>
internal static class PromiseToPaySeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.PromiseToPays.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var collectionCases = await context.CollectionCases
            .Include(c => c.Loan)
            .Take(15)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!collectionCases.Any()) return;

        var random = new Random(42);
        int promiseCount = 0;

        var paymentMethods = new[] { "BranchPayment", "BankTransfer", "GCash", "PayMaya", "Remittance" };

        foreach (var collectionCase in collectionCases)
        {
            // 1-3 promises per case
            int numPromises = random.Next(1, 4);
            for (int i = 0; i < numPromises; i++)
            {
                var promiseDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(1, 60)));
                var promisedDate = promiseDate.AddDays(random.Next(3, 14));
                var promisedAmount = Math.Round(collectionCase.TotalAmountDue * (0.2m + (decimal)random.NextDouble() * 0.5m), 2);

                var promise = PromiseToPay.Create(
                    collectionCaseId: collectionCase.Id,
                    loanId: collectionCase.LoanId,
                    memberId: collectionCase.MemberId,
                    promiseDate: promiseDate,
                    promisedPaymentDate: promisedDate,
                    promisedAmount: promisedAmount,
                    paymentMethod: paymentMethods[random.Next(paymentMethods.Length)],
                    notes: "Member committed to payment during follow-up call");

                // Determine outcome based on promised date
                if (promisedDate <= DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    var outcome = random.NextDouble();
                    if (outcome < 0.4)
                    {
                        // Promise kept
                        promise.MarkKept(promisedAmount, promisedDate.AddDays(random.Next(0, 2)));
                    }
                    else if (outcome < 0.55)
                    {
                        // Partially kept
                        promise.MarkPartial(promisedAmount * 0.5m, promisedDate.AddDays(random.Next(0, 3)));
                    }
                    else if (outcome < 0.85)
                    {
                        // Broken
                        promise.MarkBroken("Payment not received by promised date. Member unreachable.");
                    }
                    else
                    {
                        // Rescheduled
                        promise.Reschedule(promisedDate.AddDays(7), "Member requested extension due to delayed salary");
                    }
                }

                await context.PromiseToPays.AddAsync(promise, cancellationToken).ConfigureAwait(false);
                promiseCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} promises to pay", tenant, promiseCount);
    }
}
