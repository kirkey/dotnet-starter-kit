using Accounting.Application.FinancialStatements.Queries.GenerateIncomeStatement.v1;

namespace Accounting.Infrastructure.Endpoints.FinancialStatements.v1;

public static class GenerateIncomeStatementEndpoint
{
    internal static RouteHandlerBuilder MapGenerateIncomeStatementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/generate/income-statement", async (GenerateIncomeStatementQuery request, ISender mediator) =>
            {
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(nameof(GenerateIncomeStatementEndpoint))
            .WithSummary("Generate Income Statement")
            .WithDescription("Generates an income statement for a given period")
            .Produces<object>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

