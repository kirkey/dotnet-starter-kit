using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Application.ChartOfAccounts.Get.v1;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

public static class ChartOfAccountGetEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new ChartOfAccountGetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ChartOfAccountGetEndpoint))
            .WithSummary("get a chart of account by id")
            .WithDescription("get a chart of account by id")
            .Produces<ChartOfAccountDto>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
