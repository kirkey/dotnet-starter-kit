using Accounting.Application.Accruals.Queries;
using Accounting.Application.Accruals.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualGetEndpoint
{
    internal static RouteHandlerBuilder MapAccrualGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAccrualByIdQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccrualGetEndpoint))
            .WithSummary("Get an accrual by ID")
            .WithDescription("Gets the details of an accrual by its ID")
            .Produces<AccrualResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
