using Accounting.Infrastructure.Endpoints.BudgetDetails.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.BudgetDetails;

/// <summary>
/// Endpoint configuration for BudgetDetails module.
/// Provides comprehensive REST API endpoints for managing budget-details.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class BudgetDetailsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all BudgetDetails endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/budget-details").WithTags("budget-detail");

        group.MapBudgetDetailCreateEndpoint();
        group.MapBudgetDetailDeleteEndpoint();
        group.MapBudgetDetailGetEndpoint();
        group.MapBudgetDetailSearchEndpoint();
        group.MapBudgetDetailUpdateEndpoint();
    }
}
