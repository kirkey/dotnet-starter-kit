using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveBalances.v1;

public static class CreateLeaveBalanceEndpoint
{
    internal static RouteHandlerBuilder MapCreateLeaveBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateLeaveBalanceCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetLeaveBalanceEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateLeaveBalanceEndpoint))
            .WithSummary("Creates a new leave balance")
            .WithDescription("Creates a new leave balance for an employee for a specific leave type and year with opening balance")
            .Produces<CreateLeaveBalanceResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.LeaveBalances.Create")
            .MapToApiVersion(1);
    }
}
