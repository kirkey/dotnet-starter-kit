using Accounting.Application.FinancialStatements.Queries.GenerateBalanceSheet.v1;
using Accounting.Application.FinancialStatements.Queries.GenerateCashFlowStatement.v1;
using Accounting.Application.FinancialStatements.Queries.GenerateIncomeStatement.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FinancialStatements;

/// <summary>
/// Endpoint configuration for Financial Statements module.
/// </summary>
public class FinancialStatementsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Financial Statements endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/financial-statements").WithTags("financial-statements");

        // Generate Balance Sheet
        group.MapPost("/generate/balance-sheet", async (GenerateBalanceSheetQuery request, ISender mediator) =>
            {
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("GenerateBalanceSheet")
            .WithSummary("Generate Balance Sheet")
            .WithDescription("Generates a balance sheet for a given date/period")
            .Produces<BalanceSheetDto>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Generate Income Statement
        group.MapPost("/generate/income-statement", async (GenerateIncomeStatementQuery request, ISender mediator) =>
            {
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("GenerateIncomeStatement")
            .WithSummary("Generate Income Statement")
            .WithDescription("Generates an income statement for a given period")
            .Produces<IncomeStatementDto>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Generate Cash Flow Statement
        group.MapPost("/generate/cash-flow", async (GenerateCashFlowStatementQuery request, ISender mediator) =>
            {
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("GenerateCashFlowStatement")
            .WithSummary("Generate Cash Flow Statement")
            .WithDescription("Generates a cash flow statement for a given period")
            .Produces<CashFlowStatementDto>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
