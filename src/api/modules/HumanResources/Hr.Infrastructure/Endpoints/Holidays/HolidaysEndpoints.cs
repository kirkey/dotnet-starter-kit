using FSH.Starter.WebApi.HumanResources.Application.Holidays.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays;

/// <summary>
/// Endpoint routes for managing holidays.
/// </summary>
public class HolidaysEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all holiday endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/holidays").WithTags("holidays");

        group.MapPost("/", async (CreateHolidayCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetHoliday", new { id = response.Id }, response);
            })
            .WithName("CreateHolidayEndpoint")
            .WithSummary("Creates a new holiday")
            .WithDescription("Creates a new holiday with Philippines Labor Code compliance including holiday type, pay rate multiplier, and regional applicability")
            .Produces<CreateHolidayResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetHolidayRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetHolidayEndpoint")
            .WithSummary("Gets holiday by ID")
            .WithDescription("Retrieves detailed information about a specific holiday including Philippines Labor Code classification")
            .Produces<HolidayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchHolidaysRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchHolidaysEndpoint")
            .WithSummary("Searches holidays")
            .WithDescription("Searches and filters holidays with pagination support")
            .Produces<PagedList<HolidayResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateHolidayCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Route ID and request ID do not match.");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateHolidayEndpoint")
            .WithSummary("Updates a holiday")
            .WithDescription("Updates holiday information including name, date, type, and pay rate multiplier")
            .Produces<UpdateHolidayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Organization))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteHolidayCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteHolidayEndpoint")
            .WithSummary("Deletes a holiday")
            .WithDescription("Removes a holiday from the system")
            .Produces<DeleteHolidayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Organization))
            .MapToApiVersion(1);
    }
}

