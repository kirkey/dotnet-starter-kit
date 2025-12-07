using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for customer cases.
/// Creates customer service cases and complaints.
/// </summary>
internal static class CustomerCaseSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.CustomerCases.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var members = await context.Members.Take(30).ToListAsync(cancellationToken).ConfigureAwait(false);
        var staff = await context.Staff
            .Where(s => s.Status == Staff.StatusActive)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!members.Any()) return;

        var random = new Random(42);
        int caseNumber = 20001;

        var cases = new (string Subject, string Category, string Priority, string Desc, string Channel, int DaysAgo)[]
        {
            // Complaints
            ("Incorrect loan interest calculation", CustomerCase.CategoryComplaint, CustomerCase.PriorityHigh, "Member claims interest charged is higher than agreed rate in loan contract", CustomerCase.ChannelPhone, -45),
            ("ATM card not working", CustomerCase.CategoryComplaint, CustomerCase.PriorityMedium, "Member unable to withdraw from ATM, card gets rejected", CustomerCase.ChannelWalkIn, -30),
            ("Delayed loan disbursement", CustomerCase.CategoryComplaint, CustomerCase.PriorityHigh, "Loan approved but funds not credited after 5 business days", CustomerCase.ChannelEmail, -25),
            ("SMS alerts not received", CustomerCase.CategoryComplaint, CustomerCase.PriorityLow, "Member not receiving transaction SMS notifications", CustomerCase.ChannelApp, -20),
            ("Branch staff behavior", CustomerCase.CategoryComplaint, CustomerCase.PriorityCritical, "Member reports discourteous treatment by branch staff", CustomerCase.ChannelPhone, -15),
            
            // Inquiries
            ("Loan top-up eligibility", CustomerCase.CategoryInquiry, CustomerCase.PriorityLow, "Member asking about requirements for additional loan", CustomerCase.ChannelPhone, -40),
            ("Account balance discrepancy", CustomerCase.CategoryInquiry, CustomerCase.PriorityMedium, "Member requesting clarification on account balance", CustomerCase.ChannelWalkIn, -35),
            ("Insurance coverage details", CustomerCase.CategoryInquiry, CustomerCase.PriorityLow, "Inquiring about micro-insurance policy coverage", CustomerCase.ChannelEmail, -28),
            ("Mobile app registration", CustomerCase.CategoryInquiry, CustomerCase.PriorityLow, "Need help registering on mobile banking app", CustomerCase.ChannelUssd, -22),
            
            // Requests
            ("Account statement request", CustomerCase.CategoryRequest, CustomerCase.PriorityLow, "Request for certified account statement for visa application", CustomerCase.ChannelEmail, -18),
            ("Loan restructuring request", CustomerCase.CategoryRequest, CustomerCase.PriorityHigh, "Member requesting payment holiday due to job loss", CustomerCase.ChannelWalkIn, -12),
            ("Address update", CustomerCase.CategoryRequest, CustomerCase.PriorityLow, "Request to update mailing address", CustomerCase.ChannelApp, -8),
            ("Early loan settlement", CustomerCase.CategoryRequest, CustomerCase.PriorityMedium, "Inquiring about early settlement and rebate", CustomerCase.ChannelPhone, -5),
            
            // Disputes
            ("Double charge on transaction", CustomerCase.CategoryDispute, CustomerCase.PriorityHigh, "Member charged twice for the same transaction", CustomerCase.ChannelPhone, -10),
            ("Unauthorized transaction", CustomerCase.CategoryDispute, CustomerCase.PriorityCritical, "Member reports transaction they did not authorize", CustomerCase.ChannelWalkIn, -3),
            
            // Feedback
            ("Positive branch experience", CustomerCase.CategoryFeedback, CustomerCase.PriorityLow, "Member commending helpful branch staff", CustomerCase.ChannelApp, -2),
            ("Loan process improvement suggestion", CustomerCase.CategoryFeedback, CustomerCase.PriorityLow, "Suggestion to simplify loan application documents", CustomerCase.ChannelWeb, -1),
        };

        foreach (var c in cases)
        {
            var member = members[random.Next(members.Count)];
            var openedAt = DateTimeOffset.UtcNow.AddDays(c.DaysAgo);

            // Set SLA based on priority
            var slaHours = c.Priority switch
            {
                CustomerCase.PriorityCritical => 4,
                CustomerCase.PriorityHigh => 24,
                CustomerCase.PriorityMedium => 48,
                _ => 72
            };

            var customerCase = CustomerCase.Create(
                caseNumber: $"CASE-{caseNumber++:D6}",
                memberId: member.Id,
                subject: c.Subject,
                category: c.Category,
                priority: c.Priority,
                description: c.Desc,
                channel: c.Channel,
                slaHours: slaHours);

            // Assign to staff (this also sets status to InProgress and records first response)
            if (staff.Count > 0)
            {
                customerCase.Assign(staff[random.Next(staff.Count)].Id);
            }

            // Process based on age
            if (c.DaysAgo < -10)
            {
                // For older cases, assign if not already assigned, then resolve and close
                if (c.DaysAgo < -20)
                {
                    customerCase.Resolve("Issue investigated and resolved. Member notified of resolution.");
                    customerCase.Close(random.Next(3, 6), "Thank you for resolving my issue");
                }
            }

            await context.CustomerCases.AddAsync(customerCase, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} customer cases", tenant, cases.Length);
    }
}
