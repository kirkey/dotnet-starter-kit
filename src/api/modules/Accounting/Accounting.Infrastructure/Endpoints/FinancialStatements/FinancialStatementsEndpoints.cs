using Accounting.Infrastructure.Endpoints.FinancialStatements.v1;

namespace Accounting.Infrastructure.Endpoints.FinancialStatements;

/// <summary>
/// Endpoint configuration for Financial Statements module.
/// </summary>
public static class FinancialStatementsEndpoints
{
    /// <summary>
    /// Maps all Financial Statements endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapFinancialStatementsEndpoints(this IEndpointRouteBuilder app)
    {
        var financialStatementsGroup = app.MapGroup("/financial-statements")
            .WithTags("Financial-Statements")
            .WithDescription("Endpoints for generating financial statements");

        // Map financial statement generation endpoints
        financialStatementsGroup.MapGenerateBalanceSheetEndpoint();
        financialStatementsGroup.MapGenerateIncomeStatementEndpoint();
        financialStatementsGroup.MapGenerateCashFlowStatementEndpoint();

        return app;
    }
}
