using Accounting.Infrastructure.Endpoints.Budgets.v1;

namespace Accounting.Infrastructure.Endpoints.Budgets;

/// <summary>
/// Endpoint configuration for Budgets module.
/// </summary>
public static class BudgetsEndpoints
{
    /// <summary>
    /// Maps all Budgets endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapBudgetsEndpoints(this IEndpointRouteBuilder app)
    {
        var budgetsGroup = app.MapGroup("/budgets")
            .WithTags("Budgets")
            .WithDescription("Endpoints for managing budgets");

        // Version 1 endpoints
        budgetsGroup.MapBudgetCreateEndpoint();
        budgetsGroup.MapBudgetSearchEndpoint();
        budgetsGroup.MapBudgetGetEndpoint();
        budgetsGroup.MapBudgetUpdateEndpoint();
        budgetsGroup.MapBudgetDeleteEndpoint();

        return app;
    }
}
