using FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays.v1;

public static class GetHolidayEndpoint
{
    internal static RouteHandlerBuilder MapGetHolidayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetHolidayRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetHolidayEndpoint))
            .WithSummary("Gets holiday by ID")
            .WithDescription("Retrieves detailed information about a specific holiday including Philippines Labor Code classification")
            .Produces<HolidayResponse>()
            .RequirePermission("Permissions.Holidays.View")
            .MapToApiVersion(1);
    }
}

