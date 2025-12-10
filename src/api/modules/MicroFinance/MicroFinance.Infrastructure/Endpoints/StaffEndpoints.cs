using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.Staffs.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Staffs.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Staffs.Reinstate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Staffs.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Staffs.Suspend.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Staffs.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Staff.
/// </summary>
public class StaffEndpoints : CarterModule
{
    private const string CreateStaff = "CreateStaff";
    private const string GetStaff = "GetStaff";
    private const string SearchStaff = "SearchStaff";
    private const string UpdateStaff = "UpdateStaff";
    private const string SuspendStaff = "SuspendStaff";
    private const string ReinstateStaff = "ReinstateStaff";

    /// <summary>
    /// Maps all Staff endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/staff").WithTags("Staff");

        group.MapPost("/", async (CreateStaffCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/microfinance/staff/{result.Id}", result);
        })
        .WithName(CreateStaff)
        .WithSummary("Create a new staff member")
        .Produces<CreateStaffResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetStaffRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(GetStaff)
        .WithSummary("Get staff member by ID")
        .Produces<StaffResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async ([FromBody] SearchStaffCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchStaff)
        .WithSummary("Search staff members with filters and pagination")
        .Produces<PagedList<StaffResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateStaffCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(UpdateStaff)
        .WithSummary("Update a staff member")
        .Produces<UpdateStaffResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/suspend", async (DefaultIdType id, SuspendStaffCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SuspendStaff)
        .WithSummary("Suspend a staff member")
        .Produces<SuspendStaffResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Suspend, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reinstate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new ReinstateStaffCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ReinstateStaff)
        .WithSummary("Reinstate a suspended staff member")
        .Produces<ReinstateStaffResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Activate, FshResources.MicroFinance))
        .MapToApiVersion(1);
    }
}
