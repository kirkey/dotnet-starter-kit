using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for MFI configuration settings.
/// Creates default configuration for the microfinance institution.
/// </summary>
internal static class MfiConfigurationSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.MfiConfigurations.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var configs = new (string Key, string Value, string Category, string Desc, string DataType)[]
        {
            // General Settings
            ("MFI_NAME", "Samahan ng Magsasaka at Mangingisda Credit Cooperative", "General", "Name of the microfinance institution", "String"),
            ("MFI_CODE", "SMMC-COOP", "General", "Institution code", "String"),
            ("CURRENCY_CODE", "PHP", "General", "Default currency code", "String"),
            ("FISCAL_YEAR_START", "01-01", "General", "Fiscal year start (MM-DD)", "String"),
            
            // Loan Settings
            ("LOAN_MAX_AMOUNT", "500000", "Loan", "Maximum loan amount in PHP", "Decimal"),
            ("LOAN_MIN_AMOUNT", "1000", "Loan", "Minimum loan amount in PHP", "Decimal"),
            ("LOAN_MAX_TERM_MONTHS", "60", "Loan", "Maximum loan term in months", "Integer"),
            ("LOAN_GRACE_PERIOD_DAYS", "3", "Loan", "Grace period before penalty applies", "Integer"),
            ("LOAN_PENALTY_RATE", "3", "Loan", "Penalty rate percentage per month", "Decimal"),
            
            // Savings Settings
            ("SAVINGS_MIN_BALANCE", "100", "Savings", "Minimum maintaining balance", "Decimal"),
            ("SAVINGS_DORMANCY_DAYS", "365", "Savings", "Days of inactivity before dormancy", "Integer"),
            ("SAVINGS_INTEREST_CALCULATION", "DailyBalance", "Savings", "Interest calculation method", "String"),
            
            // Membership Settings
            ("MEMBER_MIN_AGE", "18", "Membership", "Minimum age for membership", "Integer"),
            ("MEMBER_MAX_AGE", "65", "Membership", "Maximum age for new membership", "Integer"),
            ("MEMBER_FEE", "200", "Membership", "One-time membership fee in PHP", "Decimal"),
            ("SHARE_CAPITAL_MIN", "500", "Membership", "Minimum share capital requirement", "Decimal"),
            
            // Security Settings
            ("SESSION_TIMEOUT_MINUTES", "30", "Security", "User session timeout in minutes", "Integer"),
            ("PASSWORD_MIN_LENGTH", "8", "Security", "Minimum password length", "Integer"),
            ("MAX_LOGIN_ATTEMPTS", "5", "Security", "Maximum failed login attempts", "Integer"),
            
            // SMS/Notification Settings
            ("SMS_ENABLED", "true", "Notification", "Enable SMS notifications", "Boolean"),
            ("EMAIL_ENABLED", "true", "Notification", "Enable email notifications", "Boolean"),
            ("NOTIFICATION_DAYS_BEFORE_DUE", "3", "Notification", "Days before due date to send reminder", "Integer"),
            
            // Collection Settings
            ("COLLECTION_START_DAY", "1", "Collection", "Days past due to start collection process", "Integer"),
            ("LEGAL_REFERRAL_DAYS", "90", "Collection", "Days past due for legal referral", "Integer"),
        };

        foreach (var cfg in configs)
        {
            var config = MfiConfiguration.Create(
                key: cfg.Key,
                value: cfg.Value,
                category: cfg.Category,
                description: cfg.Desc,
                dataType: cfg.DataType);

            await context.MfiConfigurations.AddAsync(config, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded MFI configuration settings", tenant);
    }
}
