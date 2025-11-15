using FSH.Starter.WebApi.HumanResources.Application.Holidays.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays.v1;

public static class DeleteHolidayEndpoint
{
    internal static RouteHandlerBuilder MapDeleteHolidayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteHolidayCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteHolidayEndpoint))
            .WithSummary("Deletes a holiday")
            .WithDescription("Removes a holiday from the system")
            .Produces<DeleteHolidayResponse>()
            .RequirePermission("Permissions.Holidays.Delete")
            .MapToApiVersion(1);
    }
}

