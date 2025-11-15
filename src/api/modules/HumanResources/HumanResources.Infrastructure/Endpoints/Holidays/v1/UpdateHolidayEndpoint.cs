using FSH.Starter.WebApi.HumanResources.Application.Holidays.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays.v1;

public static class UpdateHolidayEndpoint
{
    internal static RouteHandlerBuilder MapUpdateHolidayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateHolidayCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Route ID and request ID do not match.");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateHolidayEndpoint))
            .WithSummary("Updates a holiday")
            .WithDescription("Updates holiday information including name, date, type, and pay rate multiplier")
            .Produces<UpdateHolidayResponse>()
            .RequirePermission("Permissions.Holidays.Update")
            .MapToApiVersion(1);
    }
}

