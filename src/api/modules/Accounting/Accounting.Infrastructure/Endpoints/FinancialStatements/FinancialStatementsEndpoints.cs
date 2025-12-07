using Accounting.Infrastructure.Endpoints.FinancialStatements.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.FinancialStatements;

/// <summary>
/// Endpoint configuration for FinancialStatements module.
/// Provides comprehensive REST API endpoints for managing financial-statements.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class FinancialStatementsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all FinancialStatements endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/financial-statements").WithTags("financial-statement");

        group.MapGenerateBalanceSheetEndpoint();
        group.MapGenerateCashFlowStatementEndpoint();
        group.MapGenerateIncomeStatementEndpoint();
    }
}
