using FSH.Starter.WebApi.HumanResources.Application.Shifts.Create.v1;

namespace HumanResources.Infrastructure.Endpoints.Shifts.v1;

public static class CreateShiftEndpoint
{
    internal static RouteHandlerBuilder MapCreateShiftEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateShiftCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetShiftEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateShiftEndpoint))
            .WithSummary("Creates a new shift")
            .WithDescription("Creates a new shift template (morning, evening, night, etc.)")
            .Produces<CreateShiftResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.Employees.Manage")
            .MapToApiVersion(1);
    }
}

