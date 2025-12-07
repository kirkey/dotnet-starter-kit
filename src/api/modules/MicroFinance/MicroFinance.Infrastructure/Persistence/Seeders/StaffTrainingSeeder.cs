using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for staff training records.
/// Creates training history for staff members.
/// </summary>
internal static class StaffTrainingSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.StaffTrainings.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var staff = await context.Staff
            .Where(s => s.Status == Staff.StatusActive)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!staff.Any()) return;

        var random = new Random(42);
        int trainingCount = 0;

        var trainings = new (string Code, string Name, string Type, string Provider, int Hours, bool Required)[]
        {
            ("TRN-AML", "Anti-Money Laundering Training", "Compliance", "BSP Accredited", 8, true),
            ("TRN-DPA", "Data Privacy Act Compliance", "Compliance", "NPC", 4, true),
            ("TRN-LND", "Loan Processing and Documentation", "Technical", "Internal", 16, true),
            ("TRN-CUS", "Customer Service Excellence", "Soft Skills", "Internal", 8, true),
            ("TRN-COL", "Collection Best Practices", "Technical", "Internal", 12, false),
            ("TRN-FIN", "Financial Literacy Training", "Technical", "External", 8, false),
            ("TRN-MKT", "Marketing and Sales", "Soft Skills", "External", 8, false),
            ("TRN-MGT", "Leadership and Management", "Management", "External", 16, false),
            ("TRN-SYS", "Core Banking System Training", "Technical", "Vendor", 24, true),
            ("TRN-RMG", "Risk Management Fundamentals", "Technical", "Internal", 8, false),
        };

        foreach (var staffMember in staff)
        {
            // Each staff member completes 3-6 trainings
            int numTrainings = random.Next(3, 7);
            var selectedTrainings = trainings.OrderBy(_ => random.Next()).Take(numTrainings);

            foreach (var training in selectedTrainings)
            {
                var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(30, 365)));
                var endDate = startDate.AddDays(training.Hours / 8); // Assuming 8 hours per day

                var staffTraining = StaffTraining.Create(
                    staffId: staffMember.Id,
                    trainingCode: training.Code,
                    trainingName: training.Name,
                    trainingType: training.Type,
                    provider: training.Provider,
                    startDate: startDate,
                    endDate: endDate,
                    durationHours: training.Hours,
                    isRequired: training.Required);

                // Most trainings are completed
                if (random.NextDouble() > 0.1)
                {
                    var score = 70 + random.Next(0, 31); // Score between 70-100
                    staffTraining.Complete(score, score >= 75);
                    
                    if (score >= 75)
                    {
                        staffTraining.IssueCertificate(
                            $"CERT-{training.Code}-{staffMember.EmployeeNumber}",
                            endDate.AddYears(1));
                    }
                }

                await context.StaffTrainings.AddAsync(staffTraining, cancellationToken).ConfigureAwait(false);
                trainingCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} staff training records", tenant, trainingCount);
    }
}
