using Accounting.Infrastructure.Endpoints.PrepaidExpenses.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses;

/// <summary>
/// Endpoint configuration for PrepaidExpenses module.
/// Provides comprehensive REST API endpoints for managing prepaid-expenses.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class PrepaidExpensesEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all PrepaidExpenses endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/prepaid-expenses").WithTags("prepaid-expense");

        group.MapPrepaidExpenseCancelEndpoint();
        group.MapPrepaidExpenseCloseEndpoint();
        group.MapPrepaidExpenseCreateEndpoint();
        group.MapPrepaidExpenseGetEndpoint();
        group.MapPrepaidExpenseRecordAmortizationEndpoint();
        group.MapPrepaidExpenseSearchEndpoint();
        group.MapPrepaidExpenseUpdateEndpoint();
    }
}
