using Accounting.Infrastructure.Endpoints.DebitMemos.v1;

namespace Accounting.Infrastructure.Endpoints.DebitMemos;

/// <summary>
/// Endpoint configuration for Debit Memos module.
/// </summary>
public static class DebitMemosEndpoints
{
    /// <summary>
    /// Maps all Debit Memos endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapDebitMemosEndpoints(this IEndpointRouteBuilder app)
    {
        var debitMemosGroup = app.MapGroup("/debit-memos")
            .WithTags("Debit-Memos")
            .WithDescription("Endpoints for managing debit memos - used to increase receivable/payable balances");

        // Version 1 endpoints - CRUD operations
        debitMemosGroup.MapDebitMemoCreateEndpoint();
        debitMemosGroup.MapDebitMemoUpdateEndpoint();
        debitMemosGroup.MapDebitMemoGetEndpoint();
        debitMemosGroup.MapDebitMemoDeleteEndpoint();
        debitMemosGroup.MapDebitMemoSearchEndpoint();
        
        // Version 1 endpoints - Specialized operations
        debitMemosGroup.MapDebitMemoApproveEndpoint();
        debitMemosGroup.MapDebitMemoApplyEndpoint();
        debitMemosGroup.MapDebitMemoVoidEndpoint();

        return app;
    }
}
