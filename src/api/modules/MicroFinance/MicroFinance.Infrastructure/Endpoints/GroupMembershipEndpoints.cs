using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Reactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Suspend.v1;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.UpdateRole.v1;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Withdraw.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Group Memberships.
/// </summary>
public class GroupMembershipEndpoints() : CarterModule("microfinance")
{

    private const string CreateGroupMembership = "CreateGroupMembership";
    private const string GetGroupMembership = "GetGroupMembership";
    private const string GetGroupMembershipsByGroup = "GetGroupMembershipsByGroup";
    private const string GetGroupMembershipsByMember = "GetGroupMembershipsByMember";
    private const string ReactivateGroupMembership = "ReactivateGroupMembership";
    private const string SearchGroupMemberships = "SearchGroupMemberships";
    private const string SuspendGroupMembership = "SuspendGroupMembership";
    private const string UpdateMembershipRole = "UpdateMembershipRole";
    private const string WithdrawGroupMembership = "WithdrawGroupMembership";

    /// <summary>
    /// Maps all Group Membership endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var membershipsGroup = app.MapGroup("microfinance/group-memberships").WithTags("group-memberships");

        membershipsGroup.MapPost("/", async (CreateGroupMembershipCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command);
            return Results.Created($"/microfinance/group-memberships/{response.Id}", response);
        })
        .WithName(CreateGroupMembership)
        .WithSummary("Creates a new group membership")
        .Produces<CreateGroupMembershipResponse>(StatusCodes.Status201Created);

        membershipsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetGroupMembershipRequest(id));
            return Results.Ok(response);
        })
        .WithName(GetGroupMembership)
        .WithSummary("Gets a group membership by ID")
        .Produces<GroupMembershipResponse>();

        membershipsGroup.MapPost("/search", async (SearchGroupMembershipsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(SearchGroupMemberships)
        .WithSummary("Searches group memberships with pagination")
        .Produces<PagedList<GroupMembershipResponse>>();

        membershipsGroup.MapGet("/by-group/{groupId:guid}", async (Guid groupId, ISender mediator) =>
        {
            var request = new SearchGroupMembershipsCommand
            {
                GroupId = groupId,
                PageNumber = 1,
                PageSize = 100
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(GetGroupMembershipsByGroup)
        .WithSummary("Gets all memberships for a group")
        .Produces<PagedList<GroupMembershipResponse>>();

        membershipsGroup.MapGet("/by-member/{memberId:guid}", async (Guid memberId, ISender mediator) =>
        {
            var request = new SearchGroupMembershipsCommand
            {
                MemberId = memberId,
                PageNumber = 1,
                PageSize = 100
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(GetGroupMembershipsByMember)
        .WithSummary("Gets all groups a member belongs to")
        .Produces<PagedList<GroupMembershipResponse>>();

        membershipsGroup.MapPut("/{id:guid}/role", async (Guid id, UpdateMembershipRoleCommand command, ISender mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(UpdateMembershipRole)
        .WithSummary("Updates the role of a group membership")
        .Produces<UpdateMembershipRoleResponse>();

        membershipsGroup.MapPost("/{id:guid}/withdraw", async (Guid id, WithdrawMembershipCommand command, ISender mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(WithdrawGroupMembership)
        .WithSummary("Withdraws a member from the group")
        .Produces<WithdrawMembershipResponse>();

        membershipsGroup.MapPost("/{id:guid}/suspend", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new SuspendMembershipCommand(id));
            return Results.Ok(response);
        })
        .WithName(SuspendGroupMembership)
        .WithSummary("Suspends a group membership")
        .Produces<SuspendMembershipResponse>();

        membershipsGroup.MapPost("/{id:guid}/reactivate", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new ReactivateMembershipCommand(id));
            return Results.Ok(response);
        })
        .WithName(ReactivateGroupMembership)
            .WithSummary("Reactivates a suspended group membership")
            .Produces<ReactivateMembershipResponse>();
    }
}
