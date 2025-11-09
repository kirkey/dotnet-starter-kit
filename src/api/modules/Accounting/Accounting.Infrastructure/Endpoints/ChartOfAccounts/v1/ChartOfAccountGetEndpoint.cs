using Accounting.Application.ChartOfAccounts.Get.v1;
using Accounting.Application.ChartOfAccounts.Responses;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

public static class ChartOfAccountGetEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetChartOfAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ChartOfAccountGetEndpoint))
            .WithSummary("Get a chart of account by ID")
            .WithDescription("Retrieves a chart of account by its unique identifier")
            .Produces<ChartOfAccountResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
