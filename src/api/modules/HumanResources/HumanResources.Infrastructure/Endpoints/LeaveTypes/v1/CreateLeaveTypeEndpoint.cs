using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveTypes.v1;

public static class CreateLeaveTypeEndpoint
{
    internal static RouteHandlerBuilder MapCreateLeaveTypeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateLeaveTypeCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetLeaveTypeEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateLeaveTypeEndpoint))
            .WithSummary("Creates a new leave type")
            .WithDescription("Creates a new leave type with Philippines Labor Code compliance including classification, accrual frequency, and approval requirements")
            .Produces<CreateLeaveTypeResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.LeaveTypes.Create")
            .MapToApiVersion(1);
    }
}
