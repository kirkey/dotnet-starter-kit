using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for communication templates.
/// Creates SMS and email templates for various member notifications.
/// </summary>
internal static class CommunicationTemplateSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 12;
        var existingCount = await context.CommunicationTemplates.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var templateData = new (string Code, string Name, string Channel, string Category, string? Subject, string Body)[]
        {
            // Welcome Messages
            ("TPL-WEL-SMS", "Welcome SMS", CommunicationTemplate.ChannelSms, CommunicationTemplate.CategoryWelcome, 
                null, "Maligayang pagdating sa MFI, {{MemberName}}! Ang iyong member ID ay {{MemberId}}. Bumisita sa pinakamalapit na branch upang makumpleto ang registration."),
            ("TPL-WEL-EMAIL", "Welcome Email", CommunicationTemplate.ChannelEmail, CommunicationTemplate.CategoryWelcome,
                "Maligayang Pagdating sa MFI - Kumpirmado na ang Iyong Membership",
                "Mahal naming {{MemberName}},\n\nMaligayang pagdating sa aming microfinance family! Aktibo na ang iyong membership.\n\nMember ID: {{MemberId}}\nBranch: {{BranchName}}\n\nBumisita sa amin upang malaman ang aming savings at loan products.\n\nMaraming salamat,\nMFI Team"),

            // Loan Notifications
            ("TPL-LOAN-APP-SMS", "Loan Application SMS", CommunicationTemplate.ChannelSms, CommunicationTemplate.CategoryLoan,
                null, "Mahal naming {{MemberName}}, natanggap na ang iyong loan application ({{LoanNumber}}) na {{Currency}}{{Amount}}. Ipoproseso ito sa loob ng 3 araw."),
            ("TPL-LOAN-APR-SMS", "Loan Approved SMS", CommunicationTemplate.ChannelSms, CommunicationTemplate.CategoryLoan,
                null, "Binabati kita {{MemberName}}! Ang iyong loan {{LoanNumber}} na {{Currency}}{{Amount}} ay naaprubahan na. Pumunta sa branch para sa disbursement."),
            ("TPL-LOAN-DIS-SMS", "Loan Disbursed SMS", CommunicationTemplate.ChannelSms, CommunicationTemplate.CategoryLoan,
                null, "Mahal naming {{MemberName}}, nailabas na ang {{Currency}}{{Amount}} sa iyong account. Loan: {{LoanNumber}}. Unang bayad sa: {{DueDate}}."),
            ("TPL-LOAN-REJ-EMAIL", "Loan Rejected Email", CommunicationTemplate.ChannelEmail, CommunicationTemplate.CategoryLoan,
                "Loan Application Status - {{LoanNumber}}",
                "Mahal naming {{MemberName}},\n\nIkinakalungkot naming ipaalam na ang iyong loan application {{LoanNumber}} ay hindi naaprubahan sa ngayon.\n\nDahilan: {{RejectionReason}}\n\nPakibisita ang iyong branch upang talakayin ang ibang opsyon.\n\nMaraming salamat,\nMFI Loans Team"),

            // Payment Reminders
            ("TPL-REM-DUE-SMS", "Payment Due Reminder SMS", CommunicationTemplate.ChannelSms, CommunicationTemplate.CategoryReminder,
                null, "Paalala: Ang iyong loan payment na {{Currency}}{{Amount}} ay due sa {{DueDate}}. Loan: {{LoanNumber}}. Magbayad ng tama upang mapanatili ang good standing."),
            ("TPL-REM-OVER-SMS", "Overdue Reminder SMS", CommunicationTemplate.ChannelSms, CommunicationTemplate.CategoryReminder,
                null, "IMPORTANTE: Ang iyong loan {{LoanNumber}} ay {{DaysOverdue}} araw nang overdue. Outstanding: {{Currency}}{{Amount}}. Mangyaring magbayad agad upang maiwasan ang penalties."),

            // Savings Notifications
            ("TPL-SAV-DEP-SMS", "Deposit Confirmation SMS", CommunicationTemplate.ChannelSms, CommunicationTemplate.CategorySavings,
                null, "Kumpirmado ang deposit: {{Currency}}{{Amount}} sa account {{AccountNumber}}. Bagong balance: {{Currency}}{{Balance}}. Salamat sa pag-iipon sa MFI."),
            ("TPL-SAV-WTH-SMS", "Withdrawal Confirmation SMS", CommunicationTemplate.ChannelSms, CommunicationTemplate.CategorySavings,
                null, "Withdrawal: {{Currency}}{{Amount}} mula sa account {{AccountNumber}}. Natitirang balance: {{Currency}}{{Balance}}. Ref: {{Reference}}."),

            // OTP
            ("TPL-OTP-SMS", "OTP Verification SMS", CommunicationTemplate.ChannelSms, CommunicationTemplate.CategoryOtp,
                null, "Ang iyong MFI verification code ay {{OtpCode}}. Valid sa loob ng 5 minuto. Huwag ibahagi ang code na ito sa iba."),

            // Collection
            ("TPL-COLL-FINAL-EMAIL", "Final Collection Notice Email", CommunicationTemplate.ChannelEmail, CommunicationTemplate.CategoryCollection,
                "Huling Paalala - Kailangan ang Bayad sa Overdue Loan",
                "Mahal naming {{MemberName}},\n\nIto ang huling paalala tungkol sa iyong overdue loan {{LoanNumber}}.\n\nOutstanding Amount: {{Currency}}{{Amount}}\nDays Overdue: {{DaysOverdue}}\n\nMangyaring bayaran ang halagang ito sa loob ng 7 araw upang maiwasan ang karagdagang aksyon.\n\nMakipag-ugnayan sa amin kaagad upang talakayin ang payment arrangements.\n\nCollection Department"),
        };

        foreach (var data in templateData)
        {
            if (await context.CommunicationTemplates.AnyAsync(t => t.Code == data.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var template = CommunicationTemplate.Create(
                code: data.Code,
                name: data.Name,
                channel: data.Channel,
                category: data.Category,
                body: data.Body,
                subject: data.Subject);

            // Activate the template
            template.Activate();

            await context.CommunicationTemplates.AddAsync(template, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} communication templates (SMS and email for loans, savings, reminders)", tenant, targetCount);
    }
}
