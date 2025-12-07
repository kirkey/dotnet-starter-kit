using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for collection actions.
/// Creates collection activity records for delinquent loans.
/// </summary>
internal static class CollectionActionSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.CollectionActions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var collectionCases = await context.CollectionCases
            .Take(15)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!collectionCases.Any()) return;

        var staff = await context.Staff
            .Where(s => s.Status == Staff.StatusActive)
            .Take(10)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var random = new Random(42);
        int actionCount = 0;

        var actionTemplates = new (string Type, string ContactMethod, string Desc, string Outcome)[]
        {
            // Phone calls
            (CollectionAction.TypePhoneCall, "Mobile", "Initial reminder call regarding overdue payment", CollectionAction.OutcomeContacted),
            (CollectionAction.TypePhoneCall, "Mobile", "Follow-up call - no answer", CollectionAction.OutcomeNoAnswer),
            (CollectionAction.TypePhoneCall, "Landline", "Called alternate number, spoke with family member", CollectionAction.OutcomeLeftMessage),
            (CollectionAction.TypePhoneCall, "Mobile", "Borrower requested extension, committed to pay", CollectionAction.OutcomePromiseToPay),
            
            // SMS
            (CollectionAction.TypeSms, "SMS", "Automated SMS reminder sent for overdue payment", CollectionAction.OutcomeMessageSent),
            (CollectionAction.TypeSms, "Viber", "Viber message sent with payment reminder", CollectionAction.OutcomeMessageSent),
            
            // Field visits
            (CollectionAction.TypeFieldVisit, "InPerson", "Visited residence, borrower not home", CollectionAction.OutcomeNotHome),
            (CollectionAction.TypeFieldVisit, "InPerson", "Met with borrower, discussed payment options", CollectionAction.OutcomeContacted),
            (CollectionAction.TypeFieldVisit, "InPerson", "Partial payment collected during visit", CollectionAction.OutcomePaymentReceived),
            
            // Letters and notices
            (CollectionAction.TypeLetter, "Mail", "First reminder letter sent via registered mail", CollectionAction.OutcomeMessageSent),
            (CollectionAction.TypeDemandNotice, "Mail", "Demand notice sent - 15 day ultimatum", CollectionAction.OutcomeMessageSent),
            
            // Guarantor contact
            (CollectionAction.TypeGuarantorContact, "Mobile", "Called guarantor to inform of delinquency", CollectionAction.OutcomeContacted),
            (CollectionAction.TypeGuarantorContact, "InPerson", "Met with guarantor, agreed to assist", CollectionAction.OutcomePromiseToPay),
        };

        foreach (var collectionCase in collectionCases)
        {
            // 3-6 actions per case
            int numActions = random.Next(3, 7);
            var selectedTemplates = actionTemplates.OrderBy(_ => random.Next()).Take(numActions);

            int dayOffset = 60;
            foreach (var template in selectedTemplates)
            {
                dayOffset -= random.Next(5, 15);
                var actionDate = DateTimeOffset.UtcNow.AddDays(-Math.Max(1, dayOffset));
                var performedBy = staff.Any() ? staff[random.Next(staff.Count)].Id : Guid.NewGuid();

                var action = CollectionAction.Create(
                    collectionCaseId: collectionCase.Id,
                    loanId: collectionCase.LoanId,
                    memberId: collectionCase.MemberId,
                    performedById: performedBy,
                    actionType: template.Type,
                    actionDate: actionDate,
                    description: template.Desc);

                action.SetContactDetails(
                    contactMethod: template.ContactMethod,
                    contactPerson: "Borrower",
                    phoneNumber: $"+639{random.Next(100000000, 999999999)}");

                action.RecordOutcome(template.Outcome, "Action completed as scheduled");

                // Some actions resulted in payment
                if (template.Outcome == CollectionAction.OutcomePaymentReceived)
                {
                    action.RecordPaymentReceived(1000m + random.Next(0, 5000));
                }

                await context.CollectionActions.AddAsync(action, cancellationToken).ConfigureAwait(false);
                actionCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} collection actions", tenant, actionCount);
    }
}
