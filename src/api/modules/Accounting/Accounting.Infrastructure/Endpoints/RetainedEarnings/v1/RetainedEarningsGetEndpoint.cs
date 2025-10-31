using Accounting.Application.RetainedEarnings.Responses;
using Accounting.Application.RetainedEarnings.Search.v1.Accounting.Application.RetainedEarnings.Search.v1.Accounting.Application.RetainedEarnings.Get;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsGetEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetRetainedEarningsRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RetainedEarningsGetEndpoint))
            .WithSummary("Get retained earnings by ID")
            .Produces<RetainedEarningsResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

