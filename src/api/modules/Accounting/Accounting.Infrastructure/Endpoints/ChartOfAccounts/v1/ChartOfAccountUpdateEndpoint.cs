using Accounting.Application.ChartOfAccounts.Update.v1;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

public static class ChartOfAccountUpdateEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateChartOfAccountCommand request, ISender mediator) =>
            {
                request.Id = id;
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ChartOfAccountUpdateEndpoint))
            .WithSummary("Update a chart of account")
            .WithDescription("Updates an existing chart of account")
            .Produces<DefaultIdType>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
