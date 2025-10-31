using Accounting.Application.Accruals.Create;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualCreateEndpoint
{
    internal static RouteHandlerBuilder MapAccrualCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAccrualCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/accruals/{response.Id}", response);
            })
            .WithName(nameof(AccrualCreateEndpoint))
            .WithSummary("Create an accrual")
            .WithDescription("Creates a new accrual entry")
            .Produces<CreateAccrualResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
