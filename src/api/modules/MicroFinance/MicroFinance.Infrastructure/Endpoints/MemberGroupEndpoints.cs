using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.AddMember.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Dashboard;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Dissolve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Member Groups.
/// </summary>
public class MemberGroupEndpoints : CarterModule
{

    private const string ActivateMemberGroup = "ActivateMemberGroup";
    private const string AddMemberToGroup = "AddMemberToGroup";
    private const string CreateMemberGroup = "CreateMemberGroup";
    private const string DeactivateMemberGroup = "DeactivateMemberGroup";
    private const string DissolveMemberGroup = "DissolveMemberGroup";
    private const string GetMemberGroup = "GetMemberGroup";
    private const string GetMemberGroupDashboard = "GetMemberGroupDashboard";
    private const string SearchMemberGroups = "SearchMemberGroups";
    private const string UpdateMemberGroup = "UpdateMemberGroup";

    /// <summary>
    /// Maps all Member Group endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var memberGroupsGroup = app.MapGroup("microfinance/member-groups").WithTags("Member Groups");

        memberGroupsGroup.MapPost("/", async (CreateMemberGroupCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/member-groups/{response.Id}", response);
            })
            .WithName(CreateMemberGroup)
            .WithSummary("Creates a new member group")
            .Produces<CreateMemberGroupResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        memberGroupsGroup.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetMemberGroupRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetMemberGroup)
            .WithSummary("Gets a member group by ID")
            .Produces<MemberGroupResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        memberGroupsGroup.MapPost("/search", async (SearchMemberGroupsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchMemberGroups)
            .WithSummary("Searches member groups with filters and pagination")
            .Produces<PagedList<MemberGroupResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

        memberGroupsGroup.MapPost("/{id:guid}/members", async (DefaultIdType id, AddMemberToGroupCommand command, ISender sender) =>
            {
                if (id != command.GroupId)
                {
                    return Results.BadRequest("Group ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/group-memberships/{response.MembershipId}", response);
            })
            .WithName(AddMemberToGroup)
            .WithSummary("Adds a member to the group")
            .Produces<AddMemberToGroupResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        memberGroupsGroup.MapPut("/{id:guid}", async (DefaultIdType id, UpdateMemberGroupCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UpdateMemberGroup)
            .WithSummary("Updates a member group")
            .Produces<UpdateMemberGroupResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        memberGroupsGroup.MapPost("/{id:guid}/activate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new ActivateMemberGroupCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ActivateMemberGroup)
            .WithSummary("Activates a member group")
            .Produces<ActivateMemberGroupResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        memberGroupsGroup.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new DeactivateMemberGroupCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(DeactivateMemberGroup)
            .WithSummary("Deactivates a member group")
            .Produces<DeactivateMemberGroupResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        memberGroupsGroup.MapPost("/{id:guid}/dissolve", async (DefaultIdType id, DissolveMemberGroupCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(DissolveMemberGroup)
            .WithSummary("Dissolves a member group")
            .Produces<DissolveMemberGroupResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        memberGroupsGroup.MapGet("/{id:guid}/dashboard", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetMemberGroupDashboardQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetMemberGroupDashboard)
            .WithSummary("Gets comprehensive member group dashboard analytics")
            .Produces<MemberGroupDashboardResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
