using Accounting.Infrastructure.Endpoints.BudgetDetails.v1;

namespace Accounting.Infrastructure.Endpoints.BudgetDetails;

/// <summary>
/// Endpoint configuration for Budget Details module.
/// </summary>
public static class BudgetDetailsEndpoints
{
    /// <summary>
    /// Maps all Budget Details endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapBudgetDetailsEndpoints(this IEndpointRouteBuilder app)
    {
        var budgetDetailsGroup = app.MapGroup("/budget-details")
            .WithTags("Budget-Details")
            .WithDescription("Endpoints for managing budget detail entries");

        // Version 1 endpoints will be added here when implemented
        budgetDetailsGroup.MapBudgetDetailCreateEndpoint();
        budgetDetailsGroup.MapBudgetDetailUpdateEndpoint();
        budgetDetailsGroup.MapBudgetDetailDeleteEndpoint();
        budgetDetailsGroup.MapBudgetDetailGetEndpoint();
        budgetDetailsGroup.MapBudgetDetailSearchEndpoint();

        return app;
    }
}
