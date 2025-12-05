using Accounting.Infrastructure.Endpoints.DebitMemos.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.DebitMemos;

/// <summary>
/// Endpoint configuration for DebitMemos module.
/// Provides comprehensive REST API endpoints for managing debit-memos.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class DebitMemosEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all DebitMemos endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/debit-memos").WithTags("debit-memo");

        group.MapDebitMemoApplyEndpoint();
        group.MapDebitMemoApproveEndpoint();
        group.MapDebitMemoCreateEndpoint();
        group.MapDebitMemoDeleteEndpoint();
        group.MapDebitMemoGetEndpoint();
        group.MapDebitMemoSearchEndpoint();
        group.MapDebitMemoUpdateEndpoint();
        group.MapDebitMemoVoidEndpoint();
    }
}
