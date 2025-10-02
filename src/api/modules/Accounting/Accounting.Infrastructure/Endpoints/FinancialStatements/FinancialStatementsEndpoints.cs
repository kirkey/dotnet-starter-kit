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
            .WithDescription("Endpoints for managing financial statements");

        // Version 1 endpoints will be added here when implemented
        // financialStatementsGroup.MapFinancialStatementCreateEndpoint();
        // financialStatementsGroup.MapFinancialStatementUpdateEndpoint();
        // financialStatementsGroup.MapFinancialStatementDeleteEndpoint();
        // financialStatementsGroup.MapFinancialStatementGetEndpoint();
        // financialStatementsGroup.MapFinancialStatementSearchEndpoint();

        return app;
    }
}
