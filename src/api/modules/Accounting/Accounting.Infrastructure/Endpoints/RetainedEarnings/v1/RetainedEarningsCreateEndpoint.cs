using Accounting.Application.RetainedEarnings.Create.v1;

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
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

