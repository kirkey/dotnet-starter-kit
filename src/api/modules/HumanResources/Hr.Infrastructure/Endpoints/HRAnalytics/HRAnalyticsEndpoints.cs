namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.HRAnalytics;

using v1;

/// <summary>
/// HR Analytics endpoints coordinator.
/// </summary>
public static class HrAnalyticsEndpoints
{
    /// <summary>
    /// Maps all HR analytics endpoints.
    /// </summary>
    public static void MapHrAnalyticsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr-analytics").WithTags("HR Analytics");
        group.MapGetHrAnalyticsEndpoint();
        group.MapGetDepartmentAnalyticsEndpoint();
        group.MapExportHrAnalyticsEndpoint();
    }
}

