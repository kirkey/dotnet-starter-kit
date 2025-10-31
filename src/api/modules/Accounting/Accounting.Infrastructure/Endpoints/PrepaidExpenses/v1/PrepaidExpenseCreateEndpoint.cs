using Accounting.Application.PrepaidExpenses.Create.v1;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses.v1;

public static class PrepaidExpenseCreateEndpoint
{
    internal static RouteHandlerBuilder MapPrepaidExpenseCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (PrepaidExpenseCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/prepaid-expenses/{response.Id}", response);
            })
            .WithName(nameof(PrepaidExpenseCreateEndpoint))
            .WithSummary("Create prepaid expense")
            .Produces<PrepaidExpenseCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

