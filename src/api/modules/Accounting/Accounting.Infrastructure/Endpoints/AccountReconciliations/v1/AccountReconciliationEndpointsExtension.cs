namespace Accounting.Infrastructure.Endpoints.AccountReconciliations.v1;

/// <summary>
/// Extension methods for mapping account reconciliation endpoints.
/// </summary>
public static class AccountReconciliationEndpointsExtension
{
    /// <summary>
    /// Maps all account reconciliation endpoints.
    /// </summary>
    internal static IEndpointRouteBuilder MapAccountReconciliationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/account-reconciliations")
            .WithTags("Account Reconciliations")
            .WithName("AccountReconciliations")
            .WithOpenApi();

        group.MapCreateAccountReconciliationEndpoint();
        group.MapGetAccountReconciliationEndpoint();
        group.MapSearchAccountReconciliationsEndpoint();
        group.MapUpdateAccountReconciliationEndpoint();
        group.MapApproveAccountReconciliationEndpoint();
        group.MapDeleteAccountReconciliationEndpoint();

        return endpoints;
    }
}

