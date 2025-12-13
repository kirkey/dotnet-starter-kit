using Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts;

/// <summary>
/// Endpoint configuration for Chart of Accounts module.
/// Provides comprehensive REST API endpoints for managing chart of accounts and account balances.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class ChartOfAccountsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Chart of Accounts endpoints to the route builder.
    /// Delegates to extension methods for CRUD, search, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/chart-of-accounts").WithTags("chart-of-accounts");

        // CRUD operations
        group.MapChartOfAccountCreateEndpoint();
        group.MapChartOfAccountSearchEndpoint();
        // group.MapChartOfAccountDashboardEndpoint(); // DISABLED: Dashboard endpoints need separate implementation
        group.MapChartOfAccountGetEndpoint();
        group.MapChartOfAccountUpdateEndpoint();
        group.MapChartOfAccountDeleteEndpoint();

        // Business operations
        group.MapChartOfAccountActivateEndpoint();
        group.MapChartOfAccountDeactivateEndpoint();
        group.MapChartOfAccountUpdateBalanceEndpoint();
        group.MapChartOfAccountImportEndpoint();
        group.MapChartOfAccountExportEndpoint();
    }
}
