using Accounting.Infrastructure.Endpoints.InterCompanyTransactions.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.InterCompanyTransactions;

/// <summary>
/// Endpoint configuration for InterCompanyTransactions module.
/// Provides comprehensive REST API endpoints for managing inter-company-transactions.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class InterCompanyTransactionsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all InterCompanyTransactions endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/inter-company-transactions").WithTags("inter-company-transaction");

        group.MapInterCompanyTransactionCreateEndpoint();
        group.MapInterCompanyTransactionGetEndpoint();
        group.MapInterCompanyTransactionSearchEndpoint();
    }
}
