using Accounting.Application.RetainedEarnings.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsCreateEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (RetainedEarningsCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/retained-earnings/{response.Id}", response);
            })
            .WithName(nameof(RetainedEarningsCreateEndpoint))
            .WithSummary("Create retained earnings record")
            .Produces<RetainedEarningsCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

