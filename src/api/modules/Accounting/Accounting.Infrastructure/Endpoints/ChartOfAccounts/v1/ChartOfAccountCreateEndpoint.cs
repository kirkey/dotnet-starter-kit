using Accounting.Application.ChartOfAccounts.Create.v1;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

public static class ChartOfAccountCreateEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateChartOfAccountCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ChartOfAccountCreateEndpoint))
            .WithSummary("create a chart of account")
            .WithDescription("create a chart of account")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
