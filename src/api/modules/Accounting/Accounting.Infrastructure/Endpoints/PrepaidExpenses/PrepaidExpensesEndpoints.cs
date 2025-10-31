using Accounting.Infrastructure.Endpoints.PrepaidExpenses.v1;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses;

public static class PrepaidExpensesEndpoints
{
    internal static IEndpointRouteBuilder MapPrepaidExpensesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/prepaid-expenses")
            .WithTags("Prepaid Expenses")
            .WithDescription("Endpoints for managing prepaid expenses")
            .MapToApiVersion(1);

        group.MapPrepaidExpenseCreateEndpoint();
        group.MapPrepaidExpenseGetEndpoint();
        group.MapPrepaidExpenseSearchEndpoint();

        return app;
    }
}

