using Accounting.Infrastructure.Endpoints.Budgets.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Budgets;

/// <summary>
/// Endpoint configuration for Budgets module.
/// Provides comprehensive REST API endpoints for managing budgets.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class BudgetsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Budgets endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/budgets").WithTags("budget");

        group.MapBudgetApproveEndpoint();
        group.MapBudgetCloseEndpoint();
        group.MapBudgetCreateEndpoint();
        group.MapBudgetDeleteEndpoint();
        group.MapBudgetGetEndpoint();
        group.MapBudgetSearchEndpoint();
        group.MapBudgetUpdateEndpoint();
    }
}
