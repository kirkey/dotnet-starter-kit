using Accounting.Application.RetainedEarnings.Responses;
using Accounting.Application.RetainedEarnings.Search.v1.Accounting.Application.RetainedEarnings.Search.v1;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsSearchEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchRetainedEarningsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RetainedEarningsSearchEndpoint))
            .WithSummary("Search retained earnings")
            .Produces<List<RetainedEarningsResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


