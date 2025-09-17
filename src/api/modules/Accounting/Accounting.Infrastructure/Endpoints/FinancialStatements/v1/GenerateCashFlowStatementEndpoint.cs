using Accounting.Application.FinancialStatements.Queries.GenerateCashFlowStatement.v1;

namespace Accounting.Infrastructure.Endpoints.FinancialStatements.v1;

public static class GenerateCashFlowStatementEndpoint
{
    internal static RouteHandlerBuilder MapGenerateCashFlowStatementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/generate/cash-flow", async (GenerateCashFlowStatementQuery request, ISender mediator) =>
            {
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(nameof(GenerateCashFlowStatementEndpoint))
            .WithSummary("Generate Cash Flow Statement")
            .WithDescription("Generates a cash flow statement for a given period")
            .Produces<object>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

