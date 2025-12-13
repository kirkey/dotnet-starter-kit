using Accounting.Application.Budgets.Dashboard;
using Accounting.Application.ChartOfAccounts.Dashboard;
using Accounting.Application.CostCenters.Dashboard;
using Accounting.Application.Customers.Dashboard;
using Accounting.Application.Projects.Dashboard;
using Accounting.Application.Vendors.Dashboard;
using Carter;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Dashboards;

/// <summary>
/// Carter module for Accounting Dashboard endpoints.
/// Groups all dashboard endpoints under a separate route prefix to avoid conflicts with CRUD endpoints.
/// Uses module-centric naming: accounting/dashboards/{entity-type}/{id}
/// </summary>
public class AccountingDashboardsEndpoints() : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/dashboards").WithTags("accounting-dashboards");

        // Chart of Account Dashboard
        group.MapGet("/chart-of-account/{accountId}", async (Guid accountId, ISender mediator) =>
        {
            var response = await mediator.Send(new GetChartOfAccountDashboardQuery(accountId)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetChartOfAccountDashboard")
        .WithSummary("Get chart of account dashboard analytics")
        .WithDescription("Retrieves comprehensive dashboard data including balance metrics, activity, transactions, and trends for a specific chart of account")
        .Produces<ChartOfAccountDashboardResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        // Vendor Dashboard
        group.MapGet("/vendor/{vendorId}", async (Guid vendorId, ISender mediator) =>
        {
            var response = await mediator.Send(new GetVendorDashboardQuery(vendorId)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetVendorDashboard")
        .WithSummary("Get vendor dashboard analytics")
        .WithDescription("Retrieves comprehensive dashboard data including financial metrics, bills, payments, and trends for a specific vendor")
        .Produces<VendorDashboardResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        // Customer Dashboard
        group.MapGet("/customer/{customerId}", async (Guid customerId, ISender mediator) =>
        {
            var response = await mediator.Send(new GetCustomerDashboardQuery(customerId)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetCustomerDashboard")
        .WithSummary("Get customer dashboard analytics")
        .WithDescription("Retrieves comprehensive dashboard data including financial metrics, invoices, payments, and trends for a specific customer")
        .Produces<CustomerDashboardResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        // Budget Dashboard
        group.MapGet("/budget/{budgetId}", async (Guid budgetId, ISender mediator) =>
        {
            var response = await mediator.Send(new GetBudgetDashboardQuery(budgetId)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetBudgetDashboard")
        .WithSummary("Get budget dashboard analytics")
        .WithDescription("Retrieves comprehensive dashboard data including budget vs actual metrics, variance analysis, and trends for a specific budget")
        .Produces<BudgetDashboardResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        // Cost Center Dashboard
        group.MapGet("/cost-center/{costCenterId}", async (Guid costCenterId, ISender mediator) =>
        {
            var response = await mediator.Send(new GetCostCenterDashboardQuery(costCenterId)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetCostCenterDashboard")
        .WithSummary("Get cost center dashboard analytics")
        .WithDescription("Retrieves comprehensive dashboard data including expense metrics, budget utilization, and trends for a specific cost center")
        .Produces<CostCenterDashboardResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        // Project Dashboard
        group.MapGet("/project/{projectId}", async (Guid projectId, ISender mediator) =>
        {
            var response = await mediator.Send(new GetProjectDashboardQuery(projectId)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetProjectDashboard")
        .WithSummary("Get project dashboard analytics")
        .WithDescription("Retrieves comprehensive dashboard data including financial metrics, budget tracking, and trends for a specific project")
        .Produces<ProjectDashboardResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}
