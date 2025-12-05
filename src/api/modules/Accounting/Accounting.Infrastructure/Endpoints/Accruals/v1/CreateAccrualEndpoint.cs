using Accounting.Application.Accruals.Create;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class CreateAccrualEndpoint
{
    internal static RouteHandlerBuilder MapCreateAccrualEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateAccrualCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Created($"/accounting/accruals/{response.Id}", response);
        })
        .WithName(nameof(CreateAccrualEndpoint))
        .WithSummary("Create an accrual")
        .WithDescription("Creates a new accrual entry")
        .Produces<CreateAccrualResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}
