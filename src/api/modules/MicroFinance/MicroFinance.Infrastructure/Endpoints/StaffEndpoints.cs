using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.Staff.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Staff.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class StaffEndpoints() : CarterModule
{

    private const string CreateStaff = "CreateStaff";
    private const string GetStaff = "GetStaff";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/staff").WithTags("Staff");

        group.MapPost("/", async (CreateStaffCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/staff/{result.Id}", result);
        })
        .WithName(CreateStaff)
        .WithSummary("Create a new staff member")
        .Produces<CreateStaffResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetStaffRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetStaff)
        .WithSummary("Get staff member by ID")
        .Produces<StaffResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
