using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for customer surveys.
/// Creates customer satisfaction surveys with response data.
/// </summary>
internal static class CustomerSurveySeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.CustomerSurveys.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var random = new Random(42);

        var surveys = new (string Title, string Type, string Desc, int DaysAgo, int Duration, bool Anonymous)[]
        {
            // NPS Surveys
            ("Q4 2024 Net Promoter Score", CustomerSurvey.TypeNPS, "Quarterly NPS measurement for overall member satisfaction", -90, 30, true),
            ("Q3 2024 Net Promoter Score", CustomerSurvey.TypeNPS, "Quarterly NPS measurement for overall member satisfaction", -180, 30, true),
            
            // Satisfaction surveys
            ("Loan Process Satisfaction", CustomerSurvey.TypeSatisfaction, "Feedback on loan application and disbursement process", -60, 45, false),
            ("Branch Service Quality", CustomerSurvey.TypeServiceQuality, "Rate your experience at our branches", -30, 30, false),
            ("Mobile App Experience", CustomerSurvey.TypeSatisfaction, "Feedback on mobile banking application", -15, 30, true),
            
            // Product feedback
            ("New Insurance Product Feedback", CustomerSurvey.TypeProductFeedback, "Gather feedback on micro-insurance offerings", -45, 60, true),
            ("Savings Product Survey", CustomerSurvey.TypeProductFeedback, "Member preferences for savings products", -20, 30, true),
            
            // Branch-specific
            ("Makati Branch Feedback", CustomerSurvey.TypeBranchFeedback, "Service quality at Makati branch", -25, 30, false),
            ("Quezon City Branch Feedback", CustomerSurvey.TypeBranchFeedback, "Service quality at QC branch", -25, 30, false),
            
            // Current active
            ("2025 Member Satisfaction Survey", CustomerSurvey.TypeSatisfaction, "Annual comprehensive member satisfaction survey", -5, 60, true),
        };

        foreach (var s in surveys)
        {
            var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(s.DaysAgo));
            var endDate = startDate.AddDays(s.Duration);

            var survey = CustomerSurvey.Create(
                title: s.Title,
                surveyType: s.Type,
                startDate: startDate,
                description: s.Desc,
                endDate: endDate,
                isAnonymous: s.Anonymous);

            survey.Update(questions: "[{\"id\":1,\"text\":\"How satisfied are you with our service?\",\"type\":\"rating\"},{\"id\":2,\"text\":\"Would you recommend us to a friend?\",\"type\":\"nps\"},{\"id\":3,\"text\":\"What could we improve?\",\"type\":\"text\"}]");
            survey.Activate();

            // Add response data for past surveys
            if (s.DaysAgo < -10)
            {
                var responses = random.Next(50, 200);
                var avgScore = 3.5m + (decimal)random.NextDouble() * 1.5m;
                
                // Record responses to update TotalResponses and AverageScore
                for (int i = 0; i < responses; i++)
                {
                    survey.RecordResponse(avgScore);
                }
                
                // Update NPS score for NPS surveys
                if (s.Type == CustomerSurvey.TypeNPS)
                {
                    survey.UpdateNpsScore(random.Next(30, 70));
                }
            }

            // Complete past surveys
            if (endDate < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                survey.Complete();
            }

            await context.CustomerSurveys.AddAsync(survey, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} customer surveys", tenant, surveys.Length);
    }
}
