using Accounting.Application.RetainedEarnings.Get;
using Accounting.Application.RetainedEarnings.Responses;

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
            .WithSummary("Get retained earnings details by ID")
            .Produces<RetainedEarningsDetailsResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

