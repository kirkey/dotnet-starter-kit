namespace FSH.Starter.Blazor.Infrastructure.Features;

public interface IFeatureFlagService
{
    bool IsEnabled(string flag);
    IReadOnlyDictionary<string,bool> All();
    Task SetAsync(string flag, bool enabled);
    Task InitializeAsync();
}

public static class FeatureFlags
{
    public const string AdvancedReports = "advanced-reports";
    public const string ExperimentalUi = "experimental-ui";
    public const string BulkUpload = "bulk-upload";
    public const string PredictiveAnalytics = "predictive-analytics";
    public static IEnumerable<string> All = new []{AdvancedReports,ExperimentalUi,BulkUpload,PredictiveAnalytics};
}

