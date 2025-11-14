using FSH.Starter.WebApi.HumanResources.Application.Shifts.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Update.v1;
using FSH.Framework.Core.Paging;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts.v1;

public static class ShiftsEndpointsV1
{
    internal static RouteHandlerBuilder MapCreateShiftEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/", async (CreateShiftCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(MapGetShiftEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(MapCreateShiftEndpoint)).WithSummary("Creates a new shift")
        .Produces<CreateShiftResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Shifts.Create").MapToApiVersion(1);

    internal static RouteHandlerBuilder MapGetShiftEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetShiftRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(MapGetShiftEndpoint)).WithSummary("Gets shift by ID")
        .Produces<ShiftResponse>()
        .RequirePermission("Permissions.Shifts.View").MapToApiVersion(1);

    internal static RouteHandlerBuilder MapSearchShiftsEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/search", async (SearchShiftsRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(MapSearchShiftsEndpoint)).WithSummary("Searches shifts")
        .Produces<PagedList<ShiftResponse>>()
        .RequirePermission("Permissions.Shifts.View").MapToApiVersion(1);

    internal static RouteHandlerBuilder MapUpdateShiftEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPut("/{id}", async (DefaultIdType id, UpdateShiftCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(MapUpdateShiftEndpoint)).WithSummary("Updates a shift")
        .Produces<UpdateShiftResponse>()
        .RequirePermission("Permissions.Shifts.Edit").MapToApiVersion(1);

    internal static RouteHandlerBuilder MapDeleteShiftEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new DeleteShiftCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(MapDeleteShiftEndpoint)).WithSummary("Deletes a shift")
        .Produces<DeleteShiftResponse>()
        .RequirePermission("Permissions.Shifts.Delete").MapToApiVersion(1);
}

