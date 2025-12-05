using Accounting.Infrastructure.Endpoints.CreditMemos.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.CreditMemos;

/// <summary>
/// Endpoint configuration for CreditMemos module.
/// Provides comprehensive REST API endpoints for managing credit-memos.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class CreditMemosEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all CreditMemos endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/credit-memos").WithTags("credit-memo");

        group.MapCreditMemoApplyEndpoint();
        group.MapCreditMemoApproveEndpoint();
        group.MapCreditMemoCreateEndpoint();
        group.MapCreditMemoDeleteEndpoint();
        group.MapCreditMemoGetEndpoint();
        group.MapCreditMemoRefundEndpoint();
        group.MapCreditMemoSearchEndpoint();
        group.MapCreditMemoUpdateEndpoint();
        group.MapCreditMemoVoidEndpoint();
    }
}
