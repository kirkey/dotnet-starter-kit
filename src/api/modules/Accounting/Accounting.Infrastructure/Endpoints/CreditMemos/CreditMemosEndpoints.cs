using Accounting.Infrastructure.Endpoints.CreditMemos.v1;

namespace Accounting.Infrastructure.Endpoints.CreditMemos;

/// <summary>
/// Endpoint configuration for Credit Memos module.
/// </summary>
public static class CreditMemosEndpoints
{
    /// <summary>
    /// Maps all Credit Memos endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapCreditMemosEndpoints(this IEndpointRouteBuilder app)
    {
        var creditMemosGroup = app.MapGroup("/credit-memos")
            .WithTags("Credit-Memos")
            .WithDescription("Endpoints for managing credit memos - used to decrease receivable/payable balances");

        // Version 1 endpoints - CRUD operations
        creditMemosGroup.MapCreditMemoCreateEndpoint();
        creditMemosGroup.MapCreditMemoUpdateEndpoint();
        creditMemosGroup.MapCreditMemoGetEndpoint();
        creditMemosGroup.MapCreditMemoDeleteEndpoint();
        creditMemosGroup.MapCreditMemoSearchEndpoint();
        
        // Version 1 endpoints - Specialized operations
        creditMemosGroup.MapCreditMemoApproveEndpoint();
        creditMemosGroup.MapCreditMemoApplyEndpoint();
        creditMemosGroup.MapCreditMemoRefundEndpoint();
        creditMemosGroup.MapCreditMemoVoidEndpoint();

        return app;
    }
}
