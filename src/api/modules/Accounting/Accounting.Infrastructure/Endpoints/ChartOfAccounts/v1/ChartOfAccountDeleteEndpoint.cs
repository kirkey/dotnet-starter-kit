using Accounting.Application.ChartOfAccounts.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

public static class ChartOfAccountDeleteEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteChartOfAccountCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(ChartOfAccountDeleteEndpoint))
            .WithSummary("delete chart of account by id")
            .WithDescription("delete chart of account by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
