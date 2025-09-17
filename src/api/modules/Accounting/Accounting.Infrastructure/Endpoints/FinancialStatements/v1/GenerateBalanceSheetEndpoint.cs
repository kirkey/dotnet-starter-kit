using Accounting.Application.FinancialStatements.Queries.GenerateBalanceSheet.v1;

namespace Accounting.Infrastructure.Endpoints.FinancialStatements.v1;

public static class GenerateBalanceSheetEndpoint
{
    internal static RouteHandlerBuilder MapGenerateBalanceSheetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/generate/balance-sheet", async (GenerateBalanceSheetQuery request, ISender mediator) =>
            {
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(nameof(GenerateBalanceSheetEndpoint))
            .WithSummary("Generate Balance Sheet")
            .WithDescription("Generates a balance sheet for a given date/period")
            .Produces<object>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

