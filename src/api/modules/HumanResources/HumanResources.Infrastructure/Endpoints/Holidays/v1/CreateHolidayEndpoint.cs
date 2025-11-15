using FSH.Starter.WebApi.HumanResources.Application.Holidays.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays.v1;

public static class CreateHolidayEndpoint
{
    internal static RouteHandlerBuilder MapCreateHolidayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateHolidayCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetHolidayEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateHolidayEndpoint))
            .WithSummary("Creates a new holiday")
            .WithDescription("Creates a new holiday with Philippines Labor Code compliance including holiday type, pay rate multiplier, and regional applicability")
            .Produces<CreateHolidayResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.Holidays.Create")
            .MapToApiVersion(1);
    }
}
