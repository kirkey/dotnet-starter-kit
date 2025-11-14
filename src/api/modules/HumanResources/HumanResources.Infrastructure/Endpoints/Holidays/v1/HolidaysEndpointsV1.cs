using FSH.Starter.WebApi.HumanResources.Application.Holidays.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Update.v1;
using FSH.Framework.Core.Paging;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays.v1;

public static class HolidaysEndpointsV1
{
    internal static RouteHandlerBuilder MapCreateHolidayEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/", async (CreateHolidayCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(MapGetHolidayEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(MapCreateHolidayEndpoint)).WithSummary("Creates a new holiday")
        .Produces<CreateHolidayResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Holidays.Create").MapToApiVersion(1);

    internal static RouteHandlerBuilder MapGetHolidayEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetHolidayRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(MapGetHolidayEndpoint)).WithSummary("Gets holiday by ID")
        .Produces<HolidayResponse>()
        .RequirePermission("Permissions.Holidays.View").MapToApiVersion(1);

    internal static RouteHandlerBuilder MapSearchHolidaysEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/search", async (SearchHolidaysRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(MapSearchHolidaysEndpoint)).WithSummary("Searches holidays")
        .Produces<PagedList<HolidayResponse>>()
        .RequirePermission("Permissions.Holidays.View").MapToApiVersion(1);

    internal static RouteHandlerBuilder MapUpdateHolidayEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPut("/{id}", async (DefaultIdType id, UpdateHolidayCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(MapUpdateHolidayEndpoint)).WithSummary("Updates a holiday")
        .Produces<UpdateHolidayResponse>()
        .RequirePermission("Permissions.Holidays.Edit").MapToApiVersion(1);

    internal static RouteHandlerBuilder MapDeleteHolidayEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new DeleteHolidayCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(MapDeleteHolidayEndpoint)).WithSummary("Deletes a holiday")
        .Produces<DeleteHolidayResponse>()
        .RequirePermission("Permissions.Holidays.Delete").MapToApiVersion(1);
}

