using Accounting.Application.Meters.Create.v1;
using Accounting.Application.Meters.Delete.v1;
using Accounting.Application.Meters.Get.v1;
using Accounting.Application.Meters.Responses;
using Accounting.Application.Meters.Search.v1;
using Accounting.Application.Meters.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Meter;

/// <summary>
/// Endpoint configuration for Meter module.
/// Provides comprehensive REST API endpoints for managing meters.
/// </summary>
public class MeterEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Meter endpoints to the route builder.
    /// Includes Create, Read, Update, Delete, and Search operations for meters.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/meters").WithTags("meters");

        // Create endpoint
        group.MapPost("/", async (CreateMeterCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("CreateMeter")
            .WithSummary("Create meter")
            .WithDescription("Creates a new meter")
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetMeterRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetMeter")
            .WithSummary("Get meter")
            .WithDescription("Retrieves a meter by ID")
            .Produces<MeterResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateMeterCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID in URL does not match ID in request body");

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateMeter")
            .WithSummary("Update meter")
            .WithDescription("Updates a meter")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteMeterCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteMeter")
            .WithSummary("Delete meter")
            .WithDescription("Deletes a meter (cannot have reading history)")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async (SearchMetersRequest request, ISender mediator) =>
            {
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("SearchMeters")
            .WithSummary("Search meters")
            .WithDescription("Search meters with filters and pagination")
            .Produces<PagedList<MeterResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
