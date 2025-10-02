using Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts;

/// <summary>
/// Endpoint configuration for Chart of Accounts module.
/// </summary>
public static class ChartOfAccountsEndpoints
{
    /// <summary>
    /// Maps all Chart of Accounts endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapChartOfAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var chartOfAccountsGroup = app.MapGroup("/chart-of-accounts")
            .WithTags("Chart-Of-Accounts")
            .WithDescription("Endpoints for managing chart of accounts");

        // Version 1 endpoints
        chartOfAccountsGroup.MapChartOfAccountCreateEndpoint();
        chartOfAccountsGroup.MapChartOfAccountUpdateEndpoint();
        chartOfAccountsGroup.MapChartOfAccountDeleteEndpoint();
        chartOfAccountsGroup.MapChartOfAccountGetEndpoint();
        chartOfAccountsGroup.MapChartOfAccountSearchEndpoint();
        chartOfAccountsGroup.MapChartOfAccountImportEndpoint();
        chartOfAccountsGroup.MapChartOfAccountExportEndpoint();

        return app;
    }
}
